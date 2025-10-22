using System.Threading;
using _VRArchery.Scripts.Runtime.Equipment;
using _VRArchery.Scripts.Runtime.Score;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Runtime.Target;
using _VRArchery.Scripts.Runtime.Tutorial;
using _VRArchery.Scripts.Runtime.UI;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using KeyBoard;
using RankingSystem.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _VRArchery.Scripts.Runtime.Stage
{
    /// <summary>
    /// ゲームの進行を管理するクラス
    /// </summary>
    public class MainSequence : MonoBehaviour
    {
        [SerializeField] private StartTargetController _startTargetController;
        [SerializeField] private TargetSpawner _targetSpawner;
        [SerializeField] private TimeController _timeController;
        [SerializeField] private ScorePresenter _scorePresenter;
        [SerializeField] private TutorialPresenter _tutorialPresenter;
        [SerializeField] private ResultUIViewer _resultUIViewer;
        [SerializeField] private GameObject _timerText;
        [SerializeField] private UiAudioPlayer  _audioPlayer;
        [SerializeField] private BowActivator _bowActivator;

        private ScoreHolder _scoreHolder;
        private RankingScoreAdaptor _rankingScoreAdaptor;

        [Inject]
        public void Construct(ScoreHolder scoreHolder, RankingScoreAdaptor  rankingScoreAdapter)
        {
            _scoreHolder  = scoreHolder;
            _rankingScoreAdaptor  = rankingScoreAdapter;
        }

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

                _audioPlayer.FadeInBGM(1);

                //初期化処理
                _startTargetController.Init();
                _tutorialPresenter.Init();
                _resultUIViewer.Init();
                _bowActivator.Init();

                _scorePresenter.gameObject.SetActive(false);
                _timerText.SetActive(false);

                //名前入力を行う
                await SceneManager.LoadSceneAsync("FirstScene_Demo", LoadSceneMode.Additive)
                    .ToUniTask(cancellationToken: cts.Token);

                var enterName = LifetimeScope.Find<KeyBoardLifetimeScope>().Container.Resolve<ICompletable>();

                //入力が完了するまで待機
                await enterName.OnComplete(cts.Token);

                _scorePresenter.gameObject.SetActive(true);

                await SceneManager.UnloadSceneAsync("FirstScene_Demo")
                    .ToUniTask(cancellationToken: cts.Token);

                //チュートリアルを開始するか選択する
                var isTutorialStart = await _tutorialPresenter.TryTutorialAsync(cts.Token);

                if (isTutorialStart)
                {
                    // チュートリアルを開始する
                    await _tutorialPresenter.StartTutorialAsync(cts.Token);
                }

                await _tutorialPresenter.HideTutorialAsync(cts.Token);

                //#if !UNITY_EDITOR
                //チュートリアル用の的が討たれるまで待機
               // await _bowActivator.ActivateBowAsync(cts.Token);
                await _startTargetController.OnStartButtonClickedAsync(cts.Token);
                //#endif

                _timerText.SetActive(true);

                //的の生成開始
                _targetSpawner.StartSpawnTargetAsync(cts.Token).Forget();

                //タイマースタート
                await _timeController.StartTimerAsync(cts.Token);

                _timerText.SetActive(false);

                CustomDebug.Log("ゲーム終了");
                cts.Cancel();

                _bowActivator.DeactivateBowAsync(destroyCancellationToken).Forget();
                //await _scorePresenter.ShowScoreAnimationAsync(destroyCancellationToken);

                _rankingScoreAdaptor.Register();
                //スコアを初期化
                _scoreHolder.InitializeScore();

            }
        }

    }
}