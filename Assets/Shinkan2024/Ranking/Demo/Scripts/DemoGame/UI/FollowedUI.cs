using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    /// <summary>
    /// ランキングDemoシーンのプレイヤーにUIを追従させるクラス
    /// </summary>
    public class FollowedUI : MonoBehaviour
    {
        //追従するUIのRectTransform
        [SerializeField] Transform _target;
        [SerializeField] private Vector2 _offset;
        public void Update()
        {
            //targetのワールド座標をスクリーン座標に変換してUIの座標とする
            Vector2 screenPos = Camera.main.WorldToScreenPoint(_target.position);
            this.transform.position = screenPos + _offset;
        }
    }
}