using System.Threading;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private AudioSource _bgmAudioSource;

        public AudioSource BGM => _bgmAudioSource;

        public float _defaultBGMVolume;

        private void Awake() => Locator.Register(this);

        private void Start() => _defaultBGMVolume = BGM.volume;

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
        /// BGMをフェードアウトする
        /// </summary>
        /// <param name="time"></param>
        public void FadeOutBGM(float time)
        {
            DOTween.To(()=>_bgmAudioSource.volume,
                x => _bgmAudioSource.volume = x,
                0.0f,
                time);
        }

        public void FadeInBGM(float time)
        {
            DOTween.To(()=>_bgmAudioSource.volume,
                x => _bgmAudioSource.volume = x,
                _defaultBGMVolume,
                time);
        }
    }
}
