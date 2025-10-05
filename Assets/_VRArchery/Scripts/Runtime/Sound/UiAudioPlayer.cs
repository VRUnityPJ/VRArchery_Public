using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Sound
{
    public class UiAudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private AudioClip _audioClipShell;
        [SerializeField] private AudioSource _audioSource;

        public void PlayCountDownShellSound() => _audioSource.PlayOneShot(_audioClipShell, 1.0f);

        public void PlayCountDownSound() => _audioSource.PlayOneShot(_audioClip, 1.0f);
    }
}
