using _VRArchery.Scripts.Runtime.Score;
using _VRArchery.Scripts.Utility;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace _VRArchery.Scripts.Runtime.Target
{
    [RequireComponent(typeof(TargetMover))]
    public class TargetCollider : MonoBehaviour
    {
        [SerializeField]
        private TargetEffect _effect;

        [Tooltip("ヒットストップさせる時間（秒）")]
        [SerializeField] private float _hitStopDuration = 0.1f;

        [Tooltip("ヒットストップ中のTime.timeScale")]
        [SerializeField] private float _hitStopTimeScale = 0.1f;

        /// <summary>
        /// ポイントを獲得する補正値(1~2の範囲がよい)
        /// </summary>
        [SerializeField] private int _getPointCorrection = 1;

        private Transform _playerPos;
        private ScoreHolder _scoreHolder;
        private TargetMover _targetMover;

        private const float LifeTimeSec = 10f;

        private void Start() => TryGetComponent(out _targetMover);

        [Inject]
        public void Construct(ScoreHolder scoreHolder, Transform playerTransform)
        {
            _scoreHolder = scoreHolder;
            _playerPos  = playerTransform;
        }

        private void Update() => transform.LookAt(_playerPos);

        private async UniTaskVoid OnTriggerEnter(Collider other)
        {
            //敵にぶつかったとき
            if (other.gameObject.CompareTag("Arrow"))
            {
                //距離に応じて加算するポイントを計算する
                var addPoint = _scoreHolder.CalculateAddScore(transform.position, _playerPos.position);
                _scoreHolder.AddScore(addPoint * _getPointCorrection);

                HitStopManager.Apply(_hitStopDuration, _hitStopTimeScale);
                CustomDebug.Log(_scoreHolder.Score);

                //消滅時のアニメーション
                await _targetMover.DestroyAnimationAsync(destroyCancellationToken);

                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Stage"))
            {
                _effect.OnStartParticle();
                Destroy(gameObject);
            }
        }
    }
}
