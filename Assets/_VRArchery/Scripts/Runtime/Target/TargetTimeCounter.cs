using System;
using System.Threading;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetTimeCounter : MonoBehaviour
    {
        [SerializeField] private float _lifeTimeSec = 10f;

        private float timeCounter = 0f;

        /// <summary>
        /// 的の制限時間をカウント
        /// </summary>
        private void Update()
        {
            timeCounter += Time.deltaTime;
            if(timeCounter>_lifeTimeSec)
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// 的の現在の時間に応じてバフをもらう
        /// </summary>
        public float GetBunusPoint()
        {
            if(timeCounter<3f)
            {
                return 1.5f;
            }
            else if(timeCounter<6f)
            {
                return 1.3f;
            }
            return 1f;
        }
    }
}