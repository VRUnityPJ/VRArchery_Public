using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetSoundPlayer : MonoBehaviour
    {
        private UiAudioPlayer  _uiAudioPlayer;

        private void Start() => _uiAudioPlayer = Locator.Resolve<UiAudioPlayer>();

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