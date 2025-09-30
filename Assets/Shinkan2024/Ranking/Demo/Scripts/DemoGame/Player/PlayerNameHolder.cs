using System;
using Ranking.Scripts;
using Ranking.Scripts.Interface;
using R3;
using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    //ランキングDemoシーンにおいてPlayerNameを保持するクラス
    public class PlayerNameHolder : MonoBehaviour,IRankingDataHolder<PlayerName>
    {
        private RankingStorage _storage;
        private ReactiveProperty<PlayerName> _playerName = new ReactiveProperty<PlayerName>(new PlayerName("Player"));
        public ReadOnlyReactiveProperty<PlayerName> PlayerName => _playerName;
        
        private void Start()
        {
            SetStorage();
        }

        public void SetStorage()
        {
            _storage = RankingStorage.instance;
        }
        
        public void SendData(PlayerName name)
        {
            _storage.UpdateData(name);
        }
        /// <summary>
        /// PlayerNameHolderのplayerNameを更新し、ストレージに保存する関数
        /// </summary>
        public void UpdatePlayerName(PlayerName playerName)
        {
            _playerName.Value = playerName;
            SendData(_playerName.Value);
        }
    }
}