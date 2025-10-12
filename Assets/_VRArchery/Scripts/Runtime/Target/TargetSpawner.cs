using System;
using System.Threading;
using _VRArchery.Scripts.Runtime.Stage;
using Cysharp.Threading.Tasks;
using UnityEngine;
using uPools;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private TargetMover _targetPrefab;

        /// <summary>
        /// 的を生成する位置
        /// </summary>
        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private TimeController _timeController;

        [SerializeField] private GameObject _particleSystem;

        [Inject]
        private IObjectResolver _objectResolver;
        private AsyncObjectPool<GameObject> _objectPool;

        //事前にパーティクルを用意しておく
        private void Start() => SharedGameObjectPool.Prewarm(_particleSystem,4);

        /// <summary>
        /// 的を生成する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask StartSpawnTargetAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _timeController.LimitTimeSec.CurrentValue > 0)
            {
                var randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                var spawnDuration = Random.Range(0.1f, 3f);
                var target = _objectResolver.Instantiate(_targetPrefab, randomPoint.position, Quaternion.identity);

                EffectPoolAsync(target.gameObject.transform.position, token).Forget();

                target.MoveAsync(token).Forget();

                await UniTask.Delay(TimeSpan.FromSeconds(spawnDuration), cancellationToken: token);
            }
        }

        private async UniTask EffectPoolAsync(Vector3 position, CancellationToken token)
        {
            var effect = SharedGameObjectPool.Rent(_particleSystem, position, Quaternion.identity);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }
            finally
            {
                SharedGameObjectPool.Return(effect);
            }
        }

    }
}