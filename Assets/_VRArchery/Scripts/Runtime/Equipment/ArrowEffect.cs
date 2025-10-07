using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class ArrowEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject _hitEffect;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Target"))
            {
                Instantiate(_hitEffect, transform.position, Quaternion.identity);
            }
        }
    }
}