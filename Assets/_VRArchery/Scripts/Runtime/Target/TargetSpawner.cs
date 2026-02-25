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
        [SerializeField] private Transform[] _spawnPointsTypeForward;
        [SerializeField] private Transform[] _spawnPointsTypeSlideRight;
        [SerializeField] private Transform[] _spawnPointsTypeSlideLeft;
        [SerializeField] private Transform[] _spawnPointsTypeFallDown;

        [SerializeField] private TimeController _timeController;

        /// <summary>
        /// 的が生成されたときに表示するパーティクル
        /// </summary>
        [SerializeField] private GameObject _particleSystem;

        private IObjectResolver _objectResolver;

        /// <summary>
        /// 普通的の出現間隔（秒）Inspectorで調整
        /// </summary>
        [SerializeField] private float _normalSpawnIntervalMin = 0.1f;
        /// <summary>
        /// 普通的の出現間隔（秒）Inspectorで調整
        /// </summary>
        [SerializeField] private float _normalSpawnIntervalMax = 5f;
        /// <summary>
        /// レア的の出現間隔（秒）Inspectorで調整
        /// </summary>
        [SerializeField] private float _rareSpawnInterval = 5f;

        private void Start()
        {
            SharedGameObjectPool.Prewarm(_particleSystem, 4);
        }

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _objectResolver = resolver;
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
                var spawnDuration = Random.Range(_normalSpawnIntervalMin, _normalSpawnIntervalMax);
                TargetType spawnType = (TargetType) Random.Range(1,4);
                await UniTask.Delay(TimeSpan.FromSeconds(spawnDuration), cancellationToken: token);

                await SpawnTargetAsync(_targetPrefab[0], spawnType,token); // 通常的
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
                TargetType spawnType = (TargetType) Random.Range(1,4);

                await SpawnTargetAsync(_targetPrefab[1], spawnType,token); // レア的
            }
        }

        /// <summary>
        /// 的を生成し、エフェクトと移動を開始する共通処理
        /// </summary>
        private async UniTask SpawnTargetAsync(TargetMover prefab,TargetType targetType ,CancellationToken token)
        {
            Transform spawnPoint = null;
            if(targetType == TargetType.Forward)
                spawnPoint = _spawnPointsTypeForward[Random.Range(0, _spawnPointsTypeForward.Length)];
            else if(targetType == TargetType.SlideLeft)
                spawnPoint = _spawnPointsTypeSlideLeft[Random.Range(0, _spawnPointsTypeSlideLeft.Length)];
            else if(targetType == TargetType.SlideRight)
                spawnPoint = _spawnPointsTypeSlideRight[Random.Range(0, _spawnPointsTypeSlideRight.Length)];
            else if(targetType == TargetType.FallDown)
                spawnPoint = _spawnPointsTypeFallDown[Random.Range(0, _spawnPointsTypeFallDown.Length)];


            var target = _objectResolver.Instantiate(prefab, spawnPoint.position, Quaternion.identity);

            EffectPoolAsync(target.transform.position, token).Forget();
            target.MoveAsync(targetType,token).Forget();
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