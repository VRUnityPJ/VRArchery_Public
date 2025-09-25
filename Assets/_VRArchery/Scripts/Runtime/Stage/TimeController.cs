using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Stage
{
    public class TimeController : MonoBehaviour
    {
        /// <summary>
        /// ゲームの制限時間(秒)
        /// </summary>
        [SerializeField]
        private SerializableReactiveProperty<float> _limitTimeSec;
        private float _initialTime;

        /// <summary>
        /// 現在の残り時間
        /// </summary>
        public ReadOnlyReactiveProperty<float> LimitTimeSec => _limitTimeSec;

        /// <summary>
        /// ゲーム残り時間のカウントダウンをする
        /// </summary>
        /// <param name="token"></param>
        public async UniTask StartTimerAsync(CancellationToken token)
        {
            //カウントダウン開始時の時間を記録
            _initialTime = _limitTimeSec.Value;

            while (!token.IsCancellationRequested && _limitTimeSec.Value > 0)
            {
                await UniTask.Yield(token);

                _limitTimeSec.Value -= Time.deltaTime;
            }

            //もとに戻す
            _limitTimeSec.Value = _initialTime;
        }
    }
}