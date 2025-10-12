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

        [Inject]
        private IObjectResolver _objectResolver;
        private AsyncObjectPool<GameObject> _objectPool;

        private float _rareAppearanceRate = 0.05f;

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
                var choice = SelectTargetByRarity();
                var target = _objectResolver.Instantiate(choice, randomPoint.position, Quaternion.identity);

                EffectPoolAsync(target.gameObject.transform.position, token).Forget();

                target.MoveAsync(token).Forget();

                await UniTask.Delay(TimeSpan.FromSeconds(spawnDuration), cancellationToken: token);
            }
        }

        /// <summary>
        /// 設定された出現率に基づいて、生成する的のプレハブを返す
        /// </summary>
        /// <returns>生成する的のプレハブ</returns>
        private TargetMover SelectTargetByRarity()
        {
            // 0.0fから1.0fの間のランダムな値を生成
            var randomValue = Random.Range(0f, 1f);

            // ランダムな値がレア出現率より小さい場合、レアな的を選択
            if (randomValue < _rareAppearanceRate)
            {
                // _targetPrefab配列の1番目をレアな的とする
                return _targetPrefab[1];
            }
            else
            {
                // それ以外の場合は、通常の的を選択
                // _targetPrefab配列の0番目を通常の的とする
                return _targetPrefab[0];
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