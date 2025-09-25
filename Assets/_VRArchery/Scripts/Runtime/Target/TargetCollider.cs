using System;
using _VRArchery.Scripts.Runtime.Score;
using UnityEngine;
using UnityEngine.Rendering;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetCollider : MonoBehaviour
    {
        [SerializeField]
        private ScoreHolder _scoreHolder;

        private void OnTriggerEnter(Collider other)
        {
            //敵にぶつかったとき
            if(other.gameObject.CompareTag("Arrow"))
            {
                _scoreHolder.AddScore(100);
                Debug.Log(_scoreHolder.Score);
            }
        }
    }
}
