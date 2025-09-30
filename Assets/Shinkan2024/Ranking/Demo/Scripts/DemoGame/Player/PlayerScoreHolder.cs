using System;
using Ranking.Scripts;
using Ranking.Scripts.Interface;
using R3;
using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    public class PlayerScoreHolder : MonoBehaviour,IRankingDataHolder<Score>
    {
        private ReactiveProperty<Score> _score = new ReactiveProperty<Score>(new Score(0));
        public ReadOnlyReactiveProperty<Score> Score => _score;
        private IRankingStorage _storage;
        private void Start()
        {
            SetStorage();
            _score.Skip(1).Subscribe(SendData).AddTo(this);
        }
        
        public void SetStorage()
        {
            _storage = RankingStorage.instance;
        }

        public void SendData(Score score)
        {
            _storage.UpdateData(score);
        }
        
        /// <summary>
        /// PanelからScoreを更新するための関数
        /// デモシーン以外では実装しないほうがいいです
        /// </summary>
        public void UpdateScore(Score score)
        {
            _score.Value = score;
        }
        public void AddScore(int num)
        {
            _score.Value =  _score.Value.Add(new Score(num));
        }
    }
}