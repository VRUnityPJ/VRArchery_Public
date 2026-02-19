using System;
using System.Threading;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetMover : MonoBehaviour
    {
        [SerializeField] private float _destroyAnimationTime = 0.5f;

        private const float LifeTimeSec = 10f;

        /// <summary>
        /// 的のアニメーションを行う
        /// </summary>
        /// <param name="token"></param>
        public async UniTask MoveAsync(TargetType targetType,CancellationToken token)
        {
            //受け取ったキャンセルトークンとdestroyCancellationTokenをくっつけて
            //どちらかがキャンセルされたら発火するキャンセルトークンを新たに生成
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token, destroyCancellationToken);
            

            if(targetType == TargetType.Forward)
            {
                try
                {
                    var randomSpeed = UnityEngine.Random.Range(4, 8);
                    var jumpPower = UnityEngine.Random.Range(3, 6);

                    var anim = DOTween.Sequence()
                        .Append(transform.DOJump(transform.position + Vector3.forward * 10f + Vector3.down * 2, jumpPower, 1, randomSpeed))
                        .ToUniTask(cancellationToken: cts.Token);

                    var lifeTimeTask = UniTask.Delay(TimeSpan.FromSeconds(LifeTimeSec), cancellationToken: cts.Token);

                    await UniTask.WhenAny(anim, lifeTimeTask);
                }
                finally
                {
                    CustomDebug.Log("動作停止");
                    if(gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else if(targetType == TargetType.SlideLeft)
            {
                try
                {
                    var randomSpeed = UnityEngine.Random.Range(4, 8);
                    var jumpPower = UnityEngine.Random.Range(0, 3);

                    var anim = DOTween.Sequence()
                        .Append(transform.DOJump(transform.position + Vector3.right * 10f + Vector3.down * 1, jumpPower, 1, randomSpeed))
                        .ToUniTask(cancellationToken: cts.Token);

                    var lifeTimeTask = UniTask.Delay(TimeSpan.FromSeconds(LifeTimeSec), cancellationToken: cts.Token);

                    await UniTask.WhenAny(anim, lifeTimeTask);
                }
                finally
                {
                    CustomDebug.Log("動作停止");
                    if(gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else if(targetType == TargetType.SlideRight)
            {
                try
                {
                    var randomSpeed = UnityEngine.Random.Range(4, 8);
                    var jumpPower = UnityEngine.Random.Range(0, 3);

                    var anim = DOTween.Sequence()
                        .Append(transform.DOJump(transform.position + Vector3.left * 10f + Vector3.down * 1, jumpPower, 1, randomSpeed))
                        .ToUniTask(cancellationToken: cts.Token);

                    var lifeTimeTask = UniTask.Delay(TimeSpan.FromSeconds(LifeTimeSec), cancellationToken: cts.Token);

                    await UniTask.WhenAny(anim, lifeTimeTask);
                }
                finally
                {
                    CustomDebug.Log("動作停止");
                    if(gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            else if(targetType == TargetType.FallDown)
            {
                try
                {
                    var randomSpeed = UnityEngine.Random.Range(4, 8);

                    var anim = DOTween.Sequence()
                        .Append(transform.DOMove(transform.position + Vector3.down * randomSpeed, LifeTimeSec).SetEase(Ease.Linear))
                        .ToUniTask(cancellationToken: cts.Token);

                    var lifeTimeTask = UniTask.Delay(TimeSpan.FromSeconds(LifeTimeSec), cancellationToken: cts.Token);

                    await UniTask.WhenAny(anim, lifeTimeTask);
                }
                finally
                {
                    CustomDebug.Log("動作停止");
                    if(gameObject != null)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 的に矢が当たって消滅するときのアニメーション
        /// </summary>
        /// <param name="token"></param>
        public async UniTask DestroyAnimationAsync(CancellationToken token)
        {
            await transform
                .DOScale(Vector3.zero, _destroyAnimationTime)
                .ToUniTask(cancellationToken: token);
        }
    }
}