using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public class ArrowFaceMovement : MonoBehaviour
    {
        public bool IsFlying { get; set; }

        private Vector3 _prePosition;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _prePosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if(IsFlying)//進行方向に回転
            {
                Vector3 velocity = transform.position - _prePosition;
                if(velocity.magnitude > 0.01f)
                {
                    transform.rotation = Quaternion.LookRotation(velocity);
                }
                _prePosition = transform.position;
            }
        }
    }
}
