using System;
using System.Threading;
using R3;
using _VRArchery.Scripts.Runtime.Score;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;
using DG.Tweening;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class ScorePresenter : MonoBehaviour
    {
        [Inject] private ScoreHolder _scoreHolder;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Start()
        {
            _scoreText.rectTransform.localScale = Vector2.zero;

            _scoreHolder.Score
                .Subscribe(value =>
                {
                    _scoreText.text = value.ToString();
                })
                .AddTo(this);
        }

        /// <summary>
        /// ゲーム終了時にスコアをアニメーションさせて表示する
        /// </summary>
        public async UniTask OnShowScoreAnimationAsync(CancellationToken token)
        {
            var seq = DOTween.Sequence();

            await seq.Append(_scoreText.rectTransform.DOScale(Vector2.one * 1.5f, 0.5f))
                .ToUniTask(cancellationToken: token);

            await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: token);

            _scoreText.rectTransform.localScale = Vector2.zero;
        }
    }
}