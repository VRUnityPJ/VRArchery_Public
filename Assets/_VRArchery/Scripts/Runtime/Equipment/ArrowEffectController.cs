using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class ArrowEffectController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _hitEffect;
        private TrailRenderer _trailRenderer;

        /// <summary>
        /// 矢の軌道を表示するかどうかのプロパティ
        /// </summary>
        public bool IsActiveTrainRenderer
        {
            get => _trailRenderer.enabled;
            set => _trailRenderer.enabled = value;
        }

        private void Start()
        {
            TryGetComponent(out _trailRenderer);
            _trailRenderer.enabled = false;
        }

        /// <summary>
        /// 的ヒット時にエフェクトを発生させる
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Target"))
            {
                Instantiate(_hitEffect, transform.position, Quaternion.identity);
            }
        }

    }
}