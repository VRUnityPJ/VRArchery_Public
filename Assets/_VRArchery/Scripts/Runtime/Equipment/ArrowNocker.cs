using System;
using System.Linq;
using _VRArchery.Scripts.Runtime.Sound;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class ArrowNocker : MonoBehaviour
    {
        private bool _canNock = false;
        private Rigidbody _rb;
        [SerializeField]private Collider _collider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Nock"))
            {
                CustomDebug.Log(" Can Nock");
                _canNock = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Nock"))
            {
                CustomDebug.Log("Cannot Nock");
                _canNock = false;
            }
        }
        public bool IsNock()
        {
            return _canNock;
        }
    }
}
