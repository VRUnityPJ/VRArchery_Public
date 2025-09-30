using Ranking.Scripts;
using Ranking.Scripts.Interface;
using R3;
using UnityEngine;

namespace KeyBoard
{
    public class PlayerNameHolder : MonoBehaviour,IRankingDataHolder<PlayerName>
    {
        private PlayerName Name;
        private InputKeyCollector _keyCollector;
        private RankingStorage _storage;
        void Start()
        {
            SetStorage();
            
            if(!TryGetComponent(out _keyCollector))
                Debug.LogError("InputKeyCollectorが取得できません");
            
            _keyCollector.TypedText.Subscribe(value =>
            {
                Name = new PlayerName(value);
                SendData(Name);
            }).AddTo(this);
        }
        public void SetStorage()
        {
            _storage = RankingStorage.instance;
        }

        public void SendData(PlayerName name)
        {
            _storage.UpdateData(name);
            Debug.Log(name.StringValue);
        }
    }
}
