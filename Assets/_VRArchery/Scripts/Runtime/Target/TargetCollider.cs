using System;
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

        private const float LifeTimeSec = 10f;

        private void OnTriggerEnter(Collider other)
        {
            //矢にぶつかったとき
            if(other.gameObject.CompareTag("Arrow"))
            {
                _scoreHolder.AddScore(100);
                Destroy(gameObject);
                CustomDebug.Log(_scoreHolder.Score);
            }
        }

        /// <summary>
        /// 的のアニメーションを行う
        /// </summary>
        /// <param name="token"></param>
        public async UniTask MoveAsync(CancellationToken token)
        {
            //受け取ったキャンセルトークンとdestroyCancellationTokenをくっつけて
            //どちらかがキャンセルされたら発火するキャンセルトークンを新たに生成
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token, destroyCancellationToken);
            DestroyAsync(LifeTimeSec, cts.Token).Forget();

            try
            {
                var seq = DOTween.Sequence();
                await seq
                    .Append(transform.DOMoveX(transform.position.x + 5, 3f))
                    .SetLoops(-1, LoopType.Yoyo)
                    .ToUniTask(cancellationToken: cts.Token);
            }
            finally
            {
                CustomDebug.Log("動作停止");
                Destroy(gameObject);
            }
        }

        private async UniTask DestroyAsync(float timeSec , CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeSec), cancellationToken: token);
            Destroy(gameObject);
        }
    }
}
