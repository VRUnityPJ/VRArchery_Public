using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class ResultUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _endText;


        private void Start() => Init();

        /// <summary>
        /// 初期化処理（View的役割）
        /// </summary>
        public void Init()
        {
            _finalScoreText.gameObject.SetActive(false);
            _rankText.gameObject.SetActive(false);
            _endText.gameObject.SetActive(false);
        }

        /// <summary>
        /// ゲーム終了時にスコアとランクを表示（Presenter的役割）
        /// </summary>
        public async UniTask ShowResultAsync(int score, CancellationToken token)
        {
            // 「やめ」表示
            _endText.gameObject.SetActive(true);
            _endText.text = "やめ";
            _endText.rectTransform.localScale = Vector3.one * 0.5f;
            _endText.rectTransform
                .DOScale(5f, 0.5f)
                .SetEase(Ease.OutBack);

            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);

            // 「やめ」非表示
            _endText.gameObject.SetActive(false);

            ShowScore(score); // View更新
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
            ShowRank(score);  // View更新
        }

        /// <summary>
        /// スコア表示（View的役割）
        /// </summary>
        private void ShowScore(int score)
        {
            _finalScoreText.gameObject.SetActive(true);
            _finalScoreText.text = $"Score : {score}";
            _finalScoreText.rectTransform.localScale = Vector3.one * 0.5f;
            _finalScoreText.rectTransform
                .DOScale(5f, 0.5f)
                .SetEase(Ease.OutBack);
        }

        /// <summary>
        /// ランク表示（View的役割）
        /// </summary>
        private void ShowRank(int score)
        {
            _rankText.gameObject.SetActive(true);
            _rankText.text = GetRankText(score); // Model的役割
            _rankText.rectTransform.localScale = Vector3.one * 0.5f;
            _rankText.rectTransform
                .DOScale(5f, 0.5f)
                .SetEase(Ease.OutBack);
        }

        /// <summary>
        /// スコアに応じたランクを返す（Model的役割）
        /// </summary>
        private string GetRankText(int score)
        {
            if (score >= 90) return "Rank : S";
            if (score >= 70) return "Rank : A";
            if (score >= 50) return "Rank : B";
            return "Rank : C";
        }
    }
}