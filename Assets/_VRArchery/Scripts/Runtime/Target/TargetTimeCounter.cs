using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Target
{
    public class TargetTimeCounter : MonoBehaviour
    {
        [SerializeField] private float _lifeTimeSec = 10f;

        private float _timeCounter = 0f;
        // 最初の的の時間ボーナスのしきい値
        [SerializeField] private float _firstBonusTime = 3f;
        // 2番目の的の時間ボーナスのしきい値
        [SerializeField] private float _secondBonusTime = 6f;
        // 最初の的の時間ボーナスの倍率
        [SerializeField] private float _firstBonusRate = 1.5f;
        // 2番目の的の時間ボーナスの倍率
        [SerializeField] private float _secondBonusRate = 1.3f;
        // 的の時間ボーナスの通常倍率
        [SerializeField] private float _normalRate = 1f;

        /// <summary>
        /// 的の制限時間をカウント
        /// </summary>
        private void Update()
        {
            _timeCounter += Time.deltaTime;
            if (_timeCounter > _lifeTimeSec)
            {
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// 的の現在の時間に応じてバフをもらう
        /// </summary>
        public float GetBonusRate()
        {
            if (_timeCounter < _firstBonusTime)
            {
                return _firstBonusRate;
            }
            else if (_timeCounter < _secondBonusTime)
            {
                return _secondBonusRate;
            }
            return _normalRate;
        }
    }
}