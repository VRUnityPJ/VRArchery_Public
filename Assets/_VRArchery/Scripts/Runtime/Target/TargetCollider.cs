using System;
using _VRArchery.Scripts.Runtime.Score;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetCollider : MonoBehaviour
    {
        [SerializeField]
        private ScoreHolder _scoreHolder;

        private void OnTriggerEnter(Collider other)
        {
            _scoreHolder.AddScore(100);
        }
    }
}
