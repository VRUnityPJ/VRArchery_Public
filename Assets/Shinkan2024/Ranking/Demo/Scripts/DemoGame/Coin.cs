using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame 
{
    public class Coin : MonoBehaviour
    {
        void Update()
        {
            //コインらしく回す
            transform.Rotate(0,100f * Time.deltaTime,0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IPlayer _player))
            {
                _player.AddScore(100);
                Destroy(gameObject);
            }
        }

    }
}
