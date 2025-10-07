using System;
using System.Threading;
using _VRArchery.Scripts.Runtime.Stage;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private TargetCollider _targetPrefab;

        /// <summary>
        /// 的を生成する位置
        /// </summary>
        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private TimeController _timeController;

        [Inject]
        private IObjectResolver _objectResolver;

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

                target.MoveAsync(token).Forget();

                await UniTask.Delay(TimeSpan.FromSeconds(spawnDuration), cancellationToken: token);
            }
        }
    }
}