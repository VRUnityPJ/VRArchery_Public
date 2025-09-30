using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    /// <summary>
    /// ランキングのDemoシーンのPlayer(球体)を管理するクラス
    /// </summary>
    public class Player : MonoBehaviour,IPlayer
    {
        private PlayerScoreHolder _holder;

        private void Start()
        {
            if(!TryGetComponent<PlayerScoreHolder>(out _holder))
                Debug.LogError("ScoreHolderが取得できません");
        }
        
        
        public void AddScore(int num)
        {
            _holder.AddScore(num);
        }
    }
}