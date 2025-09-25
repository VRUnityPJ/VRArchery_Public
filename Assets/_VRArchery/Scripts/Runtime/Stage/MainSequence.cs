using System.Threading;
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

        private readonly CancellationTokenSource _cts = new();

        private async UniTaskVoid Start() => GameStartAsync().Forget();

        private async UniTask GameStartAsync()
        {
            //ループ開始
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, _cts.Token);
                _startButtonController.Init();

                //ボタンが押されてカウントダウンが終わるまで待機
                await _startButtonController.OnStartButtonClickedAsync(cts.Token);

                //的の生成開始
                _targetSpawner.StartSpawnTargetAsync(cts.Token).Forget();

                //タイマースタート
                await _timeController.StartTimerAsync(cts.Token);

                CustomDebug.Log("ゲーム終了");
                cts.Cancel();
            }
        }

    }
}