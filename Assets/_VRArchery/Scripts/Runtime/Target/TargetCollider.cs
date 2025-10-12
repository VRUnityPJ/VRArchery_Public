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
        private TargetScoreViewer _targetScoreViewer;

        /// <summary>
        /// 的の生存期間
        /// </summary>
        private const float LifeTimeSec = 10f;

        private void Start() => TryGetComponent(out _targetMover);

        [Inject]
        public void Construct(ScoreHolder scoreHolder, Transform playerTransform, TargetScoreViewer targetScoreViewer)
        {
            _scoreHolder = scoreHolder;
            _playerPos  = playerTransform;
            _targetScoreViewer = targetScoreViewer;
        }

        private void Update() => transform.LookAt(_playerPos);

        private async UniTaskVoid OnTriggerEnter(Collider other)
        {
            //敵にぶつかったとき
            if (other.gameObject.CompareTag("Arrow"))
            {
                //距離に応じて加算するポイントを計算する
                var addPoint = _scoreHolder.CalculateAddScore(transform.position, _playerPos.position) * _getPointCorrection;
                _scoreHolder.AddScore(addPoint);
                _targetScoreViewer.ShowGetScoreAsync(addPoint, transform, destroyCancellationToken).Forget();

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
