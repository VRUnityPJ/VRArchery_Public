using System;
using R3;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Skybox
{
    /// <summary>
    /// スカイボックスを回転させるだけのクラス
    /// </summary>
    public class SkyboxRotator : MonoBehaviour
    {
        /// <summary>
        /// シェーダーの回転プロパティID
        /// </summary>
        private static readonly int s_rotation = Shader.PropertyToID("_Rotation");

        /// <summary>
        /// 回転速度
        /// </summary>
        [SerializeField]
        private float _rotationSpeed = 0.5f;

        private void Start()
        {
            // 毎フレーム実行するObservableを作成
            Observable.EveryUpdate(destroyCancellationToken)
                .Subscribe(_ =>
                {
                    var currentRotation = Time.time * _rotationSpeed;
                    // スカイボックスのマテリアルに回転値を設定
                    RenderSettings.skybox.SetFloat(s_rotation, currentRotation);
                });
        }

    }
}