using System;
using System.Threading;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class StartTargetController : MonoBehaviour
    {
        [SerializeField]
        private UiAudioPlayer _audioPlayer;

        [SerializeField]
        private GameObject _tutorialCollider;

        [SerializeField]
        private TextMeshProUGUI _countdownText;

        [SerializeField]
        private TextMeshProUGUI _instructText;

        /// <summary>
        /// コライダーの衝突判定を非同期で待つために必要
        /// </summary>
        private AsyncTriggerEnterTrigger _enterTrigger;
        private void Start() => Init();

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            _enterTrigger = _tutorialCollider.GetAsyncTriggerEnterTrigger();
            _countdownText.gameObject.SetActive(false);
            _tutorialCollider.gameObject.transform.localScale = Vector3.zero;
            _instructText.rectTransform.localScale = Vector3.zero;
        }

        /// <summary>
        /// スタートボタンを押したときにカウントダウンを開始する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnStartButtonClickedAsync(CancellationToken token)
        {
            await DOTween.Sequence()
                .Append(_tutorialCollider.gameObject.transform.DOScale(Vector3.one * 150, 0.3f))
                .Join(_instructText.rectTransform.DOScale(Vector3.one, 0.3f))
                .ToUniTask(cancellationToken: token);

            while (!token.IsCancellationRequested)
            {
                var collision = await _enterTrigger.OnTriggerEnterAsync(token);

                if (collision.gameObject.CompareTag("Arrow"))
                {
                    break;
                }
            }

            await DOTween.Sequence()
                .Append(_tutorialCollider.gameObject.transform.DOScale(Vector3.zero, 0.3f))
                .Join(_instructText.rectTransform.DOScale(Vector3.zero, 0.3f))
                .ToUniTask(cancellationToken: token);

            // カウントダウン開始
            await StartCountdownAsync(token);
        }

        private async UniTask StartCountdownAsync(CancellationToken token)
        {
            _countdownText.gameObject.SetActive(true);

            string[] countdown = { "参", "弐", "壱", "始め" };
            foreach (var count in countdown)
            {
                _countdownText.text = count;

                // スケールをリセットしてからアニメーション
                _countdownText.rectTransform.localScale = Vector3.one * 0.5f; // 小さくして

                if (count == "始め")
                {
                    _audioPlayer.PlayCountDownShellSound();
                }
                else
                {
                    _audioPlayer.PlayCountDownSound();
                }

                await _countdownText.rectTransform
                    .DOScale(1f, 0.3f)
                    .ToUniTask(cancellationToken:token); // ふわっと大きく

                await UniTask.Delay(TimeSpan.FromSeconds(0.7f), cancellationToken: token);
            }

            _countdownText.gameObject.SetActive(false);

            CustomDebug.Log("ゲームスタート！");
        }
    }
}