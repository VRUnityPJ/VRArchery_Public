using System.Threading;
using _VRArchery.Scripts.Runtime.Score;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class ScorePresenter : MonoBehaviour
    {
        [Inject] private ScoreHolder _scoreHolder;
        [SerializeField]
        private ResultUIController _resultUIController; //追加
        [SerializeField] private int _finalScore; // 最終スコア保存用

        /// <summary>
        /// ゲーム終了時にスコアをアニメーションさせて表示する
        /// </summary>
        public async UniTask OnShowScoreAnimationAsync(CancellationToken token)
        {
            await _resultUIController.ShowResultAsync(_finalScore, token);
        }
    }
}