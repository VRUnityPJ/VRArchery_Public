using System;
using System.Threading;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _VRArchery.Scripts.Runtime.UI
{
    public class StartButtonController : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private TextMeshProUGUI _countdownText;

        private void Start() => Init();

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            _countdownText.gameObject.SetActive(false);
            _startButton.gameObject.SetActive(true);

        }
        /// <summary>
        /// スタートボタンを押したときにカウントダウンを開始する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnStartButtonClickedAsync(CancellationToken token)
        {
            await _startButton.OnClickAsync(token);

            // アニメーション：ふわっと大きくなる
            await _startButton.transform.DOScale(1.2f, 0.3f)
                .SetLoops(2, LoopType.Yoyo)
                .ToUniTask(cancellationToken: token);

            // カウントダウンテキストの位置をボタンと同じにする
            _countdownText.rectTransform.position = _startButton.transform.position;

            // アニメーションが終わったらボタンを非表示にする
            _startButton.gameObject.SetActive(false);

            // カウントダウン開始
            await StartCountdownAsync(destroyCancellationToken);

            // 色変更（例：赤に変化） → これはすぐに始めてOK
            Color targetColor = Color.white;

            _startButton.image
                .DOColor(targetColor, 0.5f)
                .ToUniTask(cancellationToken: token);
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