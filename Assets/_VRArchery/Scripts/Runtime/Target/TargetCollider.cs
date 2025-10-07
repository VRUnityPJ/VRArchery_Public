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
        [SerializeField]
        private TargetEffect _effect;

        [Tooltip("ヒットストップさせる時間（秒）")]
        [SerializeField] private float _hitStopDuration = 0.1f;

        [Tooltip("ヒットストップ中のTime.timeScale")]
        [SerializeField] private float _hitStopTimeScale = 0.1f;

        private Transform _playerPos;
        private ScoreHolder _scoreHolder;

        private const float LifeTimeSec = 10f;

        [Inject]
        public void Construct(ScoreHolder scoreHolder, Transform playerTransform)
        {
            _scoreHolder = scoreHolder;
            _playerPos  = playerTransform;
        }

        private void Update() => transform.LookAt(_playerPos);

        private void OnTriggerEnter(Collider other)
        {
            //敵にぶつかったとき
            if (other.gameObject.CompareTag("Arrow"))
            {
                var addPoint = _scoreHolder.CalculateAddScore(transform.position, _playerPos.position);
                _scoreHolder.AddScore(addPoint);
                HitStopManager.Apply(_hitStopDuration, _hitStopTimeScale);
                CustomDebug.Log(_scoreHolder.Score);
                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Stage"))
            {
                _effect.OnStartParticle();
                Destroy(gameObject);
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

            try
            {
                var randomSpeed = UnityEngine.Random.Range(2, 4);

                var anim = DOTween.Sequence()
                    .Append(transform.DOJump(transform.position + Vector3.forward * 10f + Vector3.down * 2, 5f, 1, randomSpeed))
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
}
