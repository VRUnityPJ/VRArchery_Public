using _VRArchery.Scripts.Runtime.UI;
using R3;
using UnityEngine;

public class UiAudioPlayer : MonoBehaviour
{
    [SerializeField]private AudioClip _audioClip;
    [SerializeField]private AudioClip _audioClipShell;
    [SerializeField]private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void PlayCountDownShellSound()
    {
        _audioSource.PlayOneShot(_audioClipShell, 1.0f);
    }

    public void PlayCountDownSound()
    {
        _audioSource.PlayOneShot(_audioClip, 1.0f);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
