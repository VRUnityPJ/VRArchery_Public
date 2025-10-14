using System.Threading;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Sound
{
    /// <summary>
    /// 効果音やBGMを鳴らすためのクラス
    /// </summary>
    public class UiAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private AudioClip _audioClipShell;
        [SerializeField] private AudioClip _audioClipButton;
        [SerializeField] private AudioClip _audioClipTargetHit;
        [SerializeField] private AudioClip _audioClipTargetAir;
        [SerializeField] private AudioClip _makimonoStartSound;
        [SerializeField] private AudioClip _makimonoEndSound;

        [Space]
        [SerializeField] private AudioClip _cRankSound;
        [SerializeField] private AudioClip _bRankSound;
        [SerializeField] private AudioClip _aRankSound;
        [SerializeField] private AudioClip _sRankSound;

        [Space]
        [SerializeField] private AudioSource _audioSource;

        public AudioSource AudioSource => _audioSource;

        private void Awake() => Locator.Register(this);

        public void ButtonSound() => _audioSource.PlayOneShot(_audioClipButton, 1.0f);
        public void TargetHitSound() => _audioSource.PlayOneShot(_audioClipTargetHit, 1.0f);
        public void TargetAirSound() => _audioSource.PlayOneShot(_audioClipTargetAir, 1.0f);
        public void PlayCountDownShellSound() => _audioSource.PlayOneShot(_audioClipShell, 1.0f);
        public void PlayCountDownSound() => _audioSource.PlayOneShot(_audioClip, 1.0f);

        /// <summary>
        /// 巻物を広げた時の効果音
        /// </summary>
        public void PlayScrollStartSound() => _audioSource.PlayOneShot(_makimonoStartSound, 1.0f);

        /// <summary>
        /// 巻物をしまうときの効果音
        /// </summary>
        public void PlayScrollEndSound() => _audioSource.PlayOneShot(_makimonoEndSound, 1.0f);

        public void CRankSound() => _audioSource.PlayOneShot(_cRankSound, 1.0f);
        public void BRankSound() => _audioSource.PlayOneShot(_bRankSound, 1.0f);
        public void ARankSound() => _audioSource.PlayOneShot(_aRankSound, 1.0f);
        public void SRankSound() => _audioSource.PlayOneShot(_sRankSound, 1.0f);

        /// <summary>
        /// レア的が出現したときに使用する効果音
        /// レア的が破壊されたときには鳴っていた効果音を無効化する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask PlayRareSpawnSoundAsync(AudioClip clip , CancellationToken token)
        {
            // 他のサウンド再生に影響を与えないよう、元のループ設定を保持しておく
            var originalLoopState = _audioSource.loop;

            try
            {
                // レア的用のサウンドクリップを設定し、ループを有効にして再生開始
                _audioSource.clip = clip;
                _audioSource.loop = true;
                _audioSource.Play();

                // CancellationTokenがキャンセルされるまで待機する
                // これにより、レア的が破壊されるなどのイベントを外部から通知できる
                // SuppressCancellationThrow() を付けると、キャンセル時に例外が発生せず、スムーズにfinally句へ移行できる
                await UniTask.WaitUntilCanceled(cancellationToken: token)
                    .SuppressCancellationThrow();
            }
            finally
            {
                // この非同期処理の実行中に他の効果音が鳴っている可能性を考慮し、
                // 現在再生中のクリップがレア的のものである場合のみ停止する
                if (_audioSource.isPlaying && _audioSource.clip == clip)
                {
                    _audioSource.Stop();
                    _audioSource.clip = null; // クリップの参照をクリア
                }

                // AudioSourceのループ設定を元の状態に戻す
                _audioSource.loop = originalLoopState;
            }
        }
    }
}
