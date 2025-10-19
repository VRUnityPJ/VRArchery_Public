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
        [SerializeField] private TargetMover[] _targetPrefab;

        /// <summary>
        /// 的を生成する位置
        /// </summary>
        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private TimeController _timeController;

        [SerializeField] private GameObject _particleSystem;

        [Inject] private IObjectResolver _objectResolver;
        private AsyncObjectPool<GameObject> _objectPool;

        private float _rareSpawnInterval = 5f;
        private float _lastRareSpawnTime;

        //事前にパーティクルを用意しておく
        private void Start()
        {
            SharedGameObjectPool.Prewarm(_particleSystem, 4);
            _lastRareSpawnTime = Time.time - _rareSpawnInterval; // 最初のレア出現を5秒後にする
        }





        /// <summary>
        /// 的を生成する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask StartSpawnTargetAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _timeController.LimitTimeSec.CurrentValue > 0)
            {
                var currentTime = Time.time;
                // レア的の出現条件を満たしているかチェック
                if (currentTime - _lastRareSpawnTime >= _rareSpawnInterval)
                {
                    var rareSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                    var rareTarget = _objectResolver.Instantiate(_targetPrefab[1], rareSpawnPoint.position,
                        Quaternion.identity);
                    EffectPoolAsync(rareTarget.transform.position, token).Forget();

                    rareTarget.MoveAsync(token).Forget();
                    _lastRareSpawnTime = currentTime;

                }

                // 通常的の生成
                var normalSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                var normalTarget =
                    _objectResolver.Instantiate(_targetPrefab[0], normalSpawnPoint.position, Quaternion.identity);
                EffectPoolAsync(normalTarget.transform.position, token).Forget();
                normalTarget.MoveAsync(token).Forget();
                var spawnDuration = Random.Range(0.1f, 3f);
                await UniTask.Delay(TimeSpan.FromSeconds(spawnDuration), cancellationToken: token);
            }
        }




        /// <summary>
        /// 設定された出現率に基づいて、生成する的のプレハブを返す
        /// </summary>
        /// <returns>生成する的のプレハブ</returns>

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