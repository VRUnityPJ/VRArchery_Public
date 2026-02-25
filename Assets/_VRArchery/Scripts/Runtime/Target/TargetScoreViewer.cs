using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    /// <summary>
    /// 的に当たったときに獲得したスコアを表示する用のクラス
    /// </summary>
    public class TargetScoreViewer : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _showDurationJustHit = 0.5f;

        /// <summary>
        /// スコアを獲得したときに何ポイント所得できたか表示する
        /// </summary>
        public async UniTask ShowGetScoreAsync(int score, Transform parent, bool isJustHit, CancellationToken _)
        {
            var scoreCanvas= await InstantiateAsync(_canvas, parent.position, _canvas.transform.rotation)
                .ToUniTask(cancellationToken: destroyCancellationToken);

            var scoreText = scoreCanvas[0].GetComponentsInChildren<TextMeshProUGUI>();
            //配列の0番目はスコア表示用のテキストなので、そこにスコアを表示する
            scoreText[0].text = "+ " + score + "点";

            if(isJustHit){

                //配列の1番目はジャストヒット表示用のテキスト
                scoreText[1].rectTransform.localScale = Vector3.zero;
                scoreText[1].alpha = 1f;
                DOTween.Sequence()
                    .Append(scoreText[1].rectTransform.DOScale(Vector3.one, _showDurationJustHit))
                    .AppendInterval(1f)
                    .Append(scoreText[1].rectTransform.DOScale(Vector3.zero, _showDurationJustHit))
                    .ToUniTask(cancellationToken: destroyCancellationToken)
                    .Forget();
            }

            await DOTween.Sequence()
                .Append(scoreText[0].rectTransform.DOAnchorPosY(scoreText[0].rectTransform.anchoredPosition.y + 50,1f))
                .AppendInterval(1f)
                .Join(scoreText[0].DOFade(0,1f))
                .SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: destroyCancellationToken);

            Destroy(scoreCanvas[0].gameObject);
        }
    }
}