using KeyBoard;
using R3;
using UnityEngine;

namespace RankingSystem.Scripts
{
    public class PlayerNameHolder : MonoBehaviour, IRankingDataHolder<PlayerName>
    {
        private PlayerName Name;
        private InputKeyCollector _keyCollector;
        private RankingStorage _storage;

        private void Start()
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
