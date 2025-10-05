using System.Threading;
using _VRArchery.Scripts.Runtime.Target;
using _VRArchery.Scripts.Runtime.Tutorial;
using _VRArchery.Scripts.Runtime.UI;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Stage
{
    /// <summary>
    /// ゲームの進行を管理するクラス
    /// </summary>
    public class MainSequence : MonoBehaviour
    {
        [SerializeField] private StartButtonController _startButtonController;
        [SerializeField] private TargetSpawner _targetSpawner;
        [SerializeField] private TimeController _timeController;
        [SerializeField] private ScorePresenter _scorePresenter;
        [SerializeField] private TutorialPresenter _tutorialPresenter;

        private async UniTaskVoid Start() => GameStartAsync().Forget();

        /// <summary>
        /// メインゲームを進行する
        /// </summary>
        private async UniTask GameStartAsync()
        {
            //ループ開始
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

                _startButtonController.Init();
                _tutorialPresenter.Init();

                //チュートリアルを開始するか選択する
                var isTutorialStart = await _tutorialPresenter.TryTutorialAsync(cts.Token);

                if (isTutorialStart)
                {
                    // チュートリアルを開始する
                    await _tutorialPresenter.StartTutorialAsync(cts.Token);
                }

                //ボタンが押されてカウントダウンが終わるまで待機
                await _startButtonController.OnStartButtonClickedAsync(cts.Token);

                //的の生成開始
                _targetSpawner.StartSpawnTargetAsync(cts.Token).Forget();

                //タイマースタート
                await _timeController.StartTimerAsync(cts.Token);

                CustomDebug.Log("ゲーム終了");
                cts.Cancel();

                await _scorePresenter.OnShowScoreAnimationAsync(destroyCancellationToken);
            }
        }

    }
}