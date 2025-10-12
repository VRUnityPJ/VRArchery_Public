using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetScoreViewer : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        private TextMeshProUGUI _scoreText;

        /// <summary>
        /// スコアを獲得したときに何ポイント所得できたか表示する
        /// </summary>
        public async UniTask ShowGetScoreAsync(int score, Transform parent ,CancellationToken _)
        {
            var scoreCanvas= await InstantiateAsync(_canvas, parent.position, _canvas.transform.rotation)
                .ToUniTask(cancellationToken: destroyCancellationToken);

            _scoreText = scoreCanvas[0].GetComponentInChildren<TextMeshProUGUI>();
            _scoreText.text = "+ " + score + "点";

            await DOTween.Sequence()
                .Append( _scoreText.rectTransform.DOAnchorPosY(_scoreText.rectTransform.anchoredPosition.y + 50,1f))
                .AppendInterval(1f)
                .Join(_scoreText.DOFade(0,1f))
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: destroyCancellationToken);

            Destroy(scoreCanvas[0].gameObject);
        }
    }
}