using System.Threading;
using _VRArchery.Scripts.Runtime.Score;
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
        private ResultUIViewer _resultUIViewer; //追加

        /// <summary>
        /// ゲーム終了時にスコアをアニメーションさせて表示する
        /// </summary>
        public async UniTask ShowScoreAnimationAsync(CancellationToken token)
        {
            await _resultUIViewer.ShowResultAsync(
                _scoreHolder.Score.CurrentValue,
                _scoreHolder.GetRank().ToString(),
                token);
        }
    }
}