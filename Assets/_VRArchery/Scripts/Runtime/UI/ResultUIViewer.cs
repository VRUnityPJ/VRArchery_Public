using System;
using System.Threading;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class ResultUIViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _finalScoreText;
        [SerializeField] private TextMeshProUGUI _rankText;
        [SerializeField] private TextMeshProUGUI _endText;
        [SerializeField] private TextMeshProUGUI _seeYouText;

        private readonly UiAudioPlayer _audioPlayer = Locator.Resolve<UiAudioPlayer>();

        private void Start() => Init();

        /// <summary>
        /// 初期化処理（View的役割）
        /// </summary>
        public void Init()
        {
            _finalScoreText.gameObject.SetActive(false);
            _rankText.gameObject.SetActive(false);
            _endText.gameObject.SetActive(false);
            _seeYouText.gameObject.SetActive(false);
        }

        /// <summary>
        /// ゲーム終了時にスコアとランクを表示（Presenter的役割）
        /// </summary>
        public async UniTask ShowResultAsync(int score, string rank, CancellationToken token)
        {
            // 「やめ」表示
            _audioPlayer.PlayCountDownSound();

            _endText.gameObject.SetActive(true);
            _endText.text = "やめ";
            _endText.rectTransform.localScale = Vector3.one * 0.5f;

            await _endText.rectTransform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: token);

            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: token);

            // 「やめ」非表示
            _endText.gameObject.SetActive(false);

            await ShowScoreAsync(score, token); // View更新
            await ShowRankAsync(rank, token);  // View更新

            await UniTask.Delay(TimeSpan.FromSeconds(6f), cancellationToken: token);

            _finalScoreText.gameObject.SetActive(false);
            _rankText.gameObject.SetActive(false);

            await ShowSeeYouAsync(token);
            await UniTask.Delay(TimeSpan.FromSeconds(15f), cancellationToken: token);

        }

        /// <summary>
        /// スコア表示（View的役割）
        /// </summary>
        private async UniTask ShowScoreAsync(int score, CancellationToken token)
        {
            _finalScoreText.gameObject.SetActive(true);
            _finalScoreText.text = $"Score : {score}";
            _finalScoreText.rectTransform.localScale = Vector3.one * 0.5f;

            _audioPlayer.PlayCountDownSound();

            await _finalScoreText.rectTransform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: token);
        }

        /// <summary>
        /// ランク表示（View的役割）
        /// </summary>
        private async UniTask ShowRankAsync(string rank, CancellationToken token)
        {
            _rankText.gameObject.SetActive(true);
            _rankText.text = $"rank: {rank}";
            _rankText.rectTransform.localScale = Vector3.one * 0.5f;

            _audioPlayer.PlayCountDownSound();

            await _rankText.rectTransform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: token);
        }

        /// <summary>
        /// ゲーム終了時に「また来てね！」というUIを出す
        /// </summary>
        private async UniTask ShowSeeYouAsync(CancellationToken token)
        {
            _seeYouText.gameObject.SetActive(true);
            _seeYouText.text = "あそんでくれてありがとう！\nまたきてね！";
            _seeYouText.rectTransform.localScale = Vector3.one * 0.5f;

            await _seeYouText.rectTransform
            .DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
            .ToUniTask(cancellationToken: token);
        }
    }
}