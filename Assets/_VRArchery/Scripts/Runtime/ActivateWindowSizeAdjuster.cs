using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace _VRArchery.Scripts.Runtime
{
    public class ActivateWindowSizeAdjuster : MonoBehaviour
    {
        private void Awake()
        {
            #if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
            //WindowSizeAdjusterの付いたオブジェクトを生成し、DontDestroyOnLoadでシーンを跨いでも破棄されないように
            DontDestroyOnLoad(new GameObject("WindowSizeAdjuster", typeof(WindowSizeAdjuster)));
            #endif
        }
    }
}