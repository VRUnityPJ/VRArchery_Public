using System;
using System.Collections.Generic;
using _VRArchery.Scripts.Utility;
using DG.Tweening;
using UnityEngine;
using ZLinq;

namespace _VRArchery.Scripts.Runtime.Sound
{
    /// <summary>
    /// 効果音やBGMを鳴らすためのクラス
    /// </summary>
    public class UiAudioPlayer : MonoBehaviour
    {
        [Space]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioSource _bgmAudioSource;
        [SerializeField] private SEData[] _seLists;

        /// <summary>
        /// 登録されている効果音のリスト
        /// </summary>
        public IReadOnlyList<SEData> SeLists => _seLists;

        public AudioSource BGM => _bgmAudioSource;

        [SerializeField] private float _defaultBGMVolume;

        private void Awake() => Locator.Register(this);

        private void Start() => _defaultBGMVolume = BGM.volume;

        public void ButtonSound() => PlaySoundEffect(SoundEffect.Button);
        public void TargetHitSound() => PlaySoundEffect(SoundEffect.TargetHit);
        public void TargetAirSound() => PlaySoundEffect(SoundEffect.TargetAir);
        public void PlayCountDownShellSound() => PlaySoundEffect(SoundEffect.CountDownShell);
        public void PlayCountDownSound() => PlaySoundEffect(SoundEffect.CountDown);

        /// <summary>
        /// 巻物を広げた時の効果音
        /// </summary>
        public void PlayScrollStartSound() => PlaySoundEffect(SoundEffect.ScrollStart);

        /// <summary>
        /// 巻物をしまうときの効果音
        /// </summary>
        public void PlayScrollEndSound() => PlaySoundEffect(SoundEffect.ScrollEnd);

        public void CRankSound() => PlaySoundEffect(SoundEffect.CRank);
        public void BRankSound() => PlaySoundEffect(SoundEffect.BRank);
        public void ARankSound() => PlaySoundEffect(SoundEffect.ARank);
        public void SRankSound() => PlaySoundEffect(SoundEffect.SRank);

        /// <summary>
        /// 指定した効果音を鳴らす
        /// </summary>
        /// <param name="seName">効果音名を入れる、追加するときは新にSoundEffectに追加すること</param>
        public void PlaySoundEffect(SoundEffect seName)
        {
            var se = _seLists.AsValueEnumerable()
                .Where(x => x.SEName == seName)
                .FirstOrDefault();

            _audioSource.PlayOneShot(se.SEClip);
        }

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

    [Serializable]
    public class SEData
    {
        /// <summary>
        /// 効果音の名前
        /// </summary>
        public SoundEffect SEName => _seName;

        /// <summary>
        /// 効果音のデータ
        /// </summary>
        public AudioClip SEClip => _seClip;

        [SerializeField]
        private SoundEffect _seName;
        [SerializeField]
        private AudioClip _seClip;
    }
}
