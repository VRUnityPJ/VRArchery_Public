using UnityEngine;

namespace RankingSystem.Scripts
{
    /// <summary>
    /// ランキングDemoシーンのプレイヤーを動かすクラス
    /// </summary>
    public class PlayerMover : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private void Start()
        {
            if(!TryGetComponent(out _rigidbody))
                Debug.LogError("RigidBodyが取得できてません");
        }

        private void FixedUpdate()
        {
            var toX = Input.GetAxis("Horizontal");
            var toZ = Input.GetAxis("Vertical");
            _rigidbody.AddForce(new Vector3(toX,0,toZ)*0.5f);
        }
    }

}
