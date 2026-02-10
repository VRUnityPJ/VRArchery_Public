using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// 変更なし

namespace _VRArchery.Scripts.Runtime.Equipment
{
    /// <summary>
    /// 2点間の距離に応じて、XRコントローラーの振動強度を制御するコンポーネント。
    /// XRI 3.0.0以降対応版
    /// </summary>
    public class DistanceHapticFeedback : MonoBehaviour
    {
        [Header("ターゲット設定")]
        [Tooltip("距離測定の基点1（例: 左手コントローラー）")]
        [SerializeField]
        private Transform _targetA;

        [Tooltip("距離測定の基点2（例: 右手コントローラー）")]
        [SerializeField]
        private Transform _targetB;

        [Tooltip("振動させる対象のInteractor (例: LeftHand Ray Interactor)")]
        [SerializeField]
        private XRBaseInputInteractor _interactorToVibrate; // XRBaseController から変更

        [Header("振動の有効化")]
        [Tooltip("trueの場合のみ振動を実行します")]
        public bool IsVibrationEnabled = true;

        [Header("振動パラメータ設定")]
        [Tooltip("区間1（近距離）と区間2（中距離）の境目となる距離")]
        [SerializeField]
        private float _nearThreshold = 0.5f;

        [Tooltip("区間2（中距離）と区間3（遠距離）の境目となる距離")]
        [SerializeField]
        private float _farThreshold = 1.0f;

        [Tooltip("区間1（近距離）の最大振動強度 (0.0 ～ 1.0)")]
        [Range(0, 1)]
        [SerializeField]
        private float _nearMaxAmplitude = 0.2f;

        [Tooltip("区間3（遠距離）の最大振動強度 (0.0 ～ 1.0)")]
        [Range(0, 1)]
        [SerializeField]
        private float _farMaxAmplitude = 1.0f;

        [Tooltip("1回の振動の持続時間（秒）")]
        [SerializeField]
        private float _hapticDuration = 0.1f;

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        void Update()
        {
            UpdateHaptics();
        }

        /// <summary>
        /// 振動処理のメインロジック（Updateから呼び出される）
        /// </summary>
        private void UpdateHaptics()
        {
            if (!ShouldUpdateHaptics())
            {
                return;
            }

            float distance = GetDistance();
            float amplitude = CalculateAmplitude(distance);
            TriggerVibration(amplitude);
        }

        /// <summary>
        /// 振動処理を実行するための前提条件をチェックします。
        /// </summary>
        private bool ShouldUpdateHaptics()
        {
            if (!IsVibrationEnabled)
            {
                return false;
            }

            // --- ▼ 修正点 ▼ ---
            // 参照先を interactorToVibrate に変更
            if (_targetA == null || _targetB == null || _interactorToVibrate == null)
            {
                // Debug.LogWarning("DistanceHapticFeedback: ターゲットまたはInteractorが設定されていません。");
                return false;
            }
            // --- ▲ 修正点 ▲ ---

            return true;
        }

        /// <summary>
        /// 2点間の距離を計算します。
        /// </summary>
        private float GetDistance()
        {
            return Vector3.Distance(_targetA.position, _targetB.position);
        }

        /// <summary>
        /// 距離に基づいて振動強度（Amplitude）を計算します。
        /// </summary>
        private float CalculateAmplitude(float distance)
        {
            if (distance >= _farThreshold)
            {
                // --- 区間3 (遠距離) ---
                return _farMaxAmplitude;
            }

            if (distance >= _nearThreshold)
            {
                // --- 区間2 (中距離) ---
                float rangeLength = _farThreshold - _nearThreshold;
                if (rangeLength <= 0) return _nearMaxAmplitude;
                float t = (distance - _nearThreshold) / rangeLength;
                return Mathf.Lerp(_nearMaxAmplitude, _farMaxAmplitude, t);
            }

            // --- 区間1 (近距離) ---
            if (_nearThreshold <= 0) return 0;
            float t1 = distance / _nearThreshold;
            return Mathf.Lerp(0, _nearMaxAmplitude, t1);
        }

        /// <summary>
        /// 計算された強度でコントローラーの振動をトリガーします。
        /// </summary>
        private void TriggerVibration(float amplitude)
        {
            if (amplitude > 0)
            {
                // --- ▼ 修正点 ▼ ---
                // 呼び出し元を interactorToVibrate に変更
                _interactorToVibrate.SendHapticImpulse(amplitude, _hapticDuration);
                // --- ▲ 修正点 ▲ ---
            }
        }
    }
}