using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using uPools;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class BowActivator : MonoBehaviour
    {
        [SerializeField] private Transform _bow;
        [SerializeField] private ParticleSystem _activeParticle;
        [SerializeField] private float _showDuration = 0.5f;
        [SerializeField] private float _hideDuration = 0.5f;

        /// <summary>
        /// 弓の元の大きさ
        /// </summary>
        private Vector3 _originalScale;

        private void Start()
        {
            SharedGameObjectPool.Prewarm(_activeParticle.gameObject,1);
        }

        public void Init()
        {
            // _bow.gameObject.SetActive(false);
            // _bow.localScale = Vector3.zero;
        }

        /// <summary>
        /// エフェクトと同時に弓を出現させる
        /// </summary>
        /// <param name="token"></param>
        public async UniTask ActivateBowAsync(CancellationToken token)
        {
            _bow.gameObject.SetActive(true);
            var effect = SharedGameObjectPool.Rent(_activeParticle.gameObject, _bow);
            await _bow.DOScale(_originalScale, _showDuration)
                .ToUniTask(cancellationToken: token);
            SharedGameObjectPool.Return(effect);
        }

        /// <summary>
        /// 弓を非表示にする. ゲーム終了時などに呼び出す
        /// </summary>
        /// <param name="token"></param>
        public async UniTask DeactivateBowAsync(CancellationToken token)
        {
            var effect = SharedGameObjectPool.Rent(_activeParticle.gameObject, _bow);
            await _bow.DOScale(Vector3.zero, _hideDuration)
                .ToUniTask(cancellationToken: token);
            _bow.gameObject.SetActive(false);
            SharedGameObjectPool.Return(effect);
        }
    }
}