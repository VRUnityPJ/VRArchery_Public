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
        [SerializeField] private TitlePresenter _titlePresenter;
        [SerializeField] private AutoGrabController _autoGrabController;
        [SerializeField] private InteractorToggler _interactorToggler;

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
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            _audioPlayer.FadeInBGM(1);

            //初期化処理
            _startTargetController.Init();
            _tutorialPresenter.Init();
            _resultUIViewer.Init();
            _bowActivator.Init();

            _scorePresenter.gameObject.SetActive(false);
            _timerText.SetActive(false);

            //タイトル画面を表示する
            _titlePresenter.TitleUIAnimationAsync(cts.Token).Forget();
            await _titlePresenter.OnClickStartButtonAsync(cts.Token);
            await _titlePresenter.OpenShojiAnimationAsync(cts.Token);

             // //名前入力を行う
             // await SceneManager.LoadSceneAsync("FirstScene_Demo", LoadSceneMode.Additive)
             //     .ToUniTask(cancellationToken: cts.Token);
             //
             // var enterName = LifetimeScope.Find<KeyBoardLifetimeScope>().Container.Resolve<ICompletable>();
             //
             // //入力が完了するまで待機
             // await enterName.OnComplete(cts.Token);
            // await SceneManager.UnloadSceneAsync("FirstScene_Demo")
            //     .ToUniTask(cancellationToken: cts.Token);

            _scorePresenter.gameObject.SetActive(true);

            _interactorToggler.SetInput(true);

            //チュートリアルを開始するか選択する
            var isTutorialStart = await _tutorialPresenter.TryTutorialAsync(cts.Token);

            if (isTutorialStart)
            {
                //弓を出現させる
                await _bowActivator.ActivateBowAsync(cts.Token);
                // チュートリアルを開始する
                await _tutorialPresenter.StartTutorialAsync(cts.Token);
            }

            await _tutorialPresenter.HideTutorialAsync(cts.Token);

            //弓を出現させる
            await _bowActivator.ActivateBowAsync(cts.Token);

            //弓を掴ませる
            await _autoGrabController.ForceGrab(cts.Token);

            _interactorToggler.SetInput(false);

            //チュートリアル用の的が討たれるまで待機
            await _startTargetController.OnStartButtonClickedAsync(cts.Token);

            _timerText.SetActive(true);

            //的の生成開始
            _targetSpawner.StartSpawnTargetAsync(cts.Token).Forget();

            //タイマースタート
            await _timeController.StartTimerAsync(cts.Token);

            _timerText.SetActive(false);

            CustomDebug.Log("ゲーム終了");
            cts.Cancel();

            //弓を離させる
            await _autoGrabController.ForceRelease(cts.Token);

            _bowActivator.DeactivateBowAsync(destroyCancellationToken).Forget();

            //リザルト画面を表示させる
            await _scorePresenter.ShowScoreAnimationAsync(destroyCancellationToken);

            //ランキングにスコアを登録する
            // _rankingScoreAdaptor.Register();
            //スコアを初期化
            _scoreHolder.InitializeScore();

            //現在のシーンを再読み込みする
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name)
                .ToUniTask(cancellationToken: cts.Token)
                .Forget();
        }

    }
}