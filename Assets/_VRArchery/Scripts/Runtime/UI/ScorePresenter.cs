using System.Threading;
using _VRArchery.Scripts.Runtime.Score;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class ScorePresenter : MonoBehaviour
    {
        [Inject]
        private ScoreHolder _scoreHolder;

        [SerializeField]
        private ResultUIViewer _resultUIViewer;

        private UiAudioPlayer  _audioPlayer;

        private void Start() => _audioPlayer = Locator.Resolve<UiAudioPlayer>();

        /// <summary>
        /// ゲーム終了時にスコアをアニメーションさせて表示する
        /// </summary>
        public async UniTask ShowScoreAnimationAsync(CancellationToken token)
        {
            var score = _scoreHolder.Score.CurrentValue;
            var rank = _scoreHolder.GetRank().ToString();
            var needScore = ScoreHolder.GetNextRankScore(score);

            await _resultUIViewer.ShowResultAsync(score, needScore, rank, token);
            await _resultUIViewer.ShowSeeYouAsync(token);
        }
    }
}