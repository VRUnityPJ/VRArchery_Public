using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    /// <summary>
    /// 的本体にアタッチする
    /// </summary>
    public class TargetEffect : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        /// <summary>
        /// パーティクルを表示する
        /// </summary>
        public void OnStartParticle()
        {
            Instantiate(_particleSystem, transform.position, Quaternion.identity);
        }
    }
}