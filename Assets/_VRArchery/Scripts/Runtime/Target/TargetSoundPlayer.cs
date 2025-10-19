using System;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetSoundPlayer : MonoBehaviour
    {
        private UiAudioPlayer _uiAudioPlayer;

        [SerializeField]
        private AudioClip _spawnSound;

        private void Start()
        {
            _uiAudioPlayer = Locator.Resolve<UiAudioPlayer>();
        }

        /// <summary>
        /// スポーン時に使用する効果音
        /// </summary>
        private void PlaySpawnSound() => throw new Exception();

        /// <summary>
        /// 矢にふれたときに効果音を鳴らす
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Arrow"))
            {
                _uiAudioPlayer.TargetHitSound();
            }
        }
    }
}