using System;
using System.Threading;
using _VRArchery.Scripts.Runtime.Score;
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
        [SerializeField] private TextMeshProUGUI _nextRankText;
        [SerializeField] private TextMeshProUGUI _seeYouText;
        [SerializeField] private TextMeshProUGUI _creatorCommentText;

        private UiAudioPlayer _audioPlayer;

        private void Start() => Init();

        /// <summary>
        /// 初期化処理（View的役割）
        /// </summary>
        public void Init()
        {
            _audioPlayer = Locator.Resolve<UiAudioPlayer>();
            _finalScoreText.gameObject.SetActive(false);
            _rankText.gameObject.SetActive(false);
            _endText.gameObject.SetActive(false);
            _nextRankText.gameObject.SetActive(false);
            _creatorCommentText.gameObject.SetActive(false);
            _seeYouText.gameObject.SetActive(false);
        }

        /// <summary>
        /// ゲーム終了時にスコアとランクを表示（Presenter的役割）
        /// </summary>
        public async UniTask ShowResultAsync(int score, int needScore, string rank, CancellationToken token)
        {
            _audioPlayer.FadeOutBGM(5);

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
            await ShowNextRankAsync(needScore, token);

            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

            //ランクに応じて効果音を鳴らす
            switch (rank)
            {
                case nameof(Rank.半人前):
                    _audioPlayer.CRankSound();
                    break;
                case nameof(Rank.一人前):
                    _audioPlayer.BRankSound();
                    break;
                case nameof(Rank.師範代):
                    _audioPlayer.ARankSound();
                    break;
                case nameof(Rank.弓聖):
                    _audioPlayer.SRankSound();
                    break;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(6f), cancellationToken: token);

            _finalScoreText.gameObject.SetActive(false);
            _rankText.gameObject.SetActive(false);
            _nextRankText.gameObject.SetActive(false);
        }

        /// <summary>
        /// スコア表示（View的役割）
        /// </summary>
        private async UniTask ShowScoreAsync(int score, CancellationToken token)
        {
            _finalScoreText.gameObject.SetActive(true);
            _finalScoreText.text = $"スコア : {score}";
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
            _rankText.text = $"総合評価 : {rank}";
            _rankText.rectTransform.localScale = Vector3.one * 0.5f;

            _audioPlayer.PlayCountDownSound();

            await _rankText.rectTransform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: token);
        }

        /// <summary>
        /// 次のランクまで何点必要か表示するアニメーション
        /// </summary>
        /// <param name="needScore"></param>
        /// <param name="token"></param>
        private async UniTask ShowNextRankAsync(int needScore, CancellationToken token)
        {
            _nextRankText.gameObject.SetActive(true);
            _nextRankText.text = $"次のランクまで<color=\"red\">{needScore}点";
            _nextRankText.rectTransform.localScale = Vector3.zero;

            _audioPlayer.PlayCountDownSound();

            await _nextRankText.rectTransform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: token);
        }
        public async UniTask ShowCommentAsync(string rank, CancellationToken token)
        {
            _creatorCommentText.gameObject.SetActive(true);
            string comment = null;
            switch (rank)
            {
                case nameof(Rank.半人前):
                    comment = "君ならもっとできる。\nがんばれ。";
                    break;
                case nameof(Rank.一人前):
                    comment = "けっこう上手いね。\nまだ上を目指せる。";
                    break;
                case nameof(Rank.師範代):
                    comment = "才能に満ち溢れているね。\nすごい。";
                    break;
                case nameof(Rank.弓聖):
                    comment = "君は天才だ。\n開発者の僕よりうまいかも。";
                    break;
            }
            _creatorCommentText.text = comment;
            _creatorCommentText.rectTransform.localScale = Vector3.one * 0.5f;


            await _creatorCommentText.rectTransform
                .DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .ToUniTask(cancellationToken: token);
            await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
            _creatorCommentText.gameObject.SetActive(false);
        }

        /// <summary>
        /// ゲーム終了時に「また来てね！」というUIを出す
        /// </summary>
        public async UniTask ShowSeeYouAsync(CancellationToken token)
        {
            _seeYouText.gameObject.SetActive(true);

            _seeYouText.text = "あそんでくれてありがとう！\nまたきてね！";
            _seeYouText.rectTransform.localScale = Vector3.one * 0.5f;

            await _seeYouText.rectTransform
            .DOScale(1f, 0.5f)
            .SetEase(Ease.OutBack)
            .ToUniTask(cancellationToken: token);

            await UniTask.Delay(TimeSpan.FromSeconds(7f), cancellationToken: token);
        }
    }
}