using _VRArchery.Scripts.Utility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurSwitch : MonoBehaviour
{
    [SerializeField]
    private Volume _volume;
    private DepthOfField _blur;

    private void Awake()
    {
        if (_volume == null)
        {
            CustomDebug.Log("ERROR: Volume does not set");
            return;
        }
        VolumeProfile profile = _volume.sharedProfile;
        if (!profile.TryGet<DepthOfField>(out _blur))
        {
            _blur = profile.Add<DepthOfField>(true);
        }
    }

    [ContextMenu("Eneble Blur")]
    public void EnableBlur()
    {
        _blur.active = true;
    }
    
    [ContextMenu("Disable Blur")]
    public void DisableBlur()
    {
        _blur.active = false;
    }
}
