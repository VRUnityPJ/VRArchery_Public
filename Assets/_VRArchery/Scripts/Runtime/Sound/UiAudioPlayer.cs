using _VRArchery.Scripts.Utility;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Sound
{
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
    }
}
