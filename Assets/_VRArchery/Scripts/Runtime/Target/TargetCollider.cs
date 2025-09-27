using System.Threading;
using _VRArchery.Scripts.Runtime.Score;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetCollider : MonoBehaviour
    {
        [Inject]
        private ScoreHolder _scoreHolder;

        private void OnTriggerEnter(Collider other)
        {
            //敵にぶつかったとき
            if(other.gameObject.CompareTag("Arrow"))
            {
                _scoreHolder.AddScore(100);
                Debug.Log(_scoreHolder.Score);
            }
        }

        public async UniTask MoveAsync(CancellationToken token)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token, destroyCancellationToken);

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    var seq = DOTween.Sequence();

                    await seq
                        .Append(transform.DOMoveX(transform.position.x + 5, 3f))
                        .Append(transform.DOMoveX(transform.position.x - 5, 3f))
                        .ToUniTask(cancellationToken: token);
                }
            }
            finally
            {
                CustomDebug.Log("動作停止");
                Destroy(gameObject);
            }
        }
    }
}
