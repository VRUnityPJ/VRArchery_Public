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
    /// <summary>
    /// 的を自動生成するためのクラス
    /// </summary>
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private TargetMover[] _targetPrefab;

        /// <summary>
        /// 的を生成する位置
        /// </summary>
        [SerializeField] private Transform[] _spawnPoints;

        [SerializeField] private TimeController _timeController;

        /// <summary>
        /// 的が生成されたときに表示するパーティクル
        /// </summary>
        [SerializeField] private GameObject _particleSystem;

        [Inject] private IObjectResolver _objectResolver;

        /// <summary>
        /// レア的の出現間隔（秒）Inspectorで調整
        /// </summary>
        [SerializeField] private float _rareSpawnInterval = 5f;

        private void Start()
        {
            SharedGameObjectPool.Prewarm(_particleSystem, 4);
        }

        /// <summary>
        /// 的の生成処理を開始（通常的とレア的を並行して生成）
        /// </summary>
        public async UniTask StartSpawnTargetAsync(CancellationToken token)
        {
            var normalLoop = SpawnNormalTargetsAsync(token);
            var rareLoop = SpawnRareTargetsAsync(token);

            await UniTask.WhenAll(normalLoop, rareLoop);
        }

        /// <summary>
        /// 通常的をランダムな間隔で生成し続ける
        /// </summary>
        private async UniTask SpawnNormalTargetsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _timeController.LimitTimeSec.CurrentValue > 0)
            {
                var spawnDuration = Random.Range(0.1f, 3f);
                await UniTask.Delay(TimeSpan.FromSeconds(spawnDuration), cancellationToken: token);

                await SpawnTargetAsync(_targetPrefab[0], token); // 通常的
            }
        }

        /// <summary>
        /// レア的を一定間隔で生成し続ける
        /// </summary>
        private async UniTask SpawnRareTargetsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _timeController.LimitTimeSec.CurrentValue > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_rareSpawnInterval), cancellationToken: token);

                await SpawnTargetAsync(_targetPrefab[1], token); // レア的
            }
        }

        /// <summary>
        /// 的を生成し、エフェクトと移動を開始する共通処理
        /// </summary>
        private async UniTask SpawnTargetAsync(TargetMover prefab, CancellationToken token)
        {
            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            var target = _objectResolver.Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            EffectPoolAsync(target.transform.position, token).Forget();
            target.MoveAsync(token).Forget();
        }

        /// <summary>
        /// エフェクトを表示し、一定時間後にプールへ返却
        /// </summary>
        private async UniTask EffectPoolAsync(Vector3 position, CancellationToken token)
        {
            var effect = SharedGameObjectPool.Rent(_particleSystem, position, Quaternion.identity);
            effect.transform.localScale = Vector3.one;

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