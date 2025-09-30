using Ranking.Scripts.Interface;

namespace Ranking.Scripts
{
    public class PlayerName : IRankingDataElement<PlayerName>
    {
        private string _stringValue;
        public string StringValue => _stringValue;
        
        /// <summary>
        /// デフォルトコンストラクタ
        /// ランキングデータの生成に必要
        /// </summary>
        public PlayerName()
        {
            _stringValue = "";
        }
        public PlayerName(string _initialChar)
        {
            _stringValue = _initialChar;
        }

        public PlayerName(char _initial)
        {
            _stringValue = _initial.ToString();
        }

        /// <summary>
        /// 文字を連結させ、新たなPlayerNameクラスを返す
        /// 実例 new PlayerName("a").Add(new PlayerName("bc")); はabcを返す
        /// </summary>
        public PlayerName Add(PlayerName addChar)
        {
            string newName = this._stringValue + addChar.StringValue;
            return new PlayerName(newName);
        }
        
        /// <summary>
        /// 一文字消す
        /// </summary>
        public PlayerName DeleteLastChar()
        {
            var newName = _stringValue.Remove(_stringValue.Length-1);
            return new PlayerName(newName);
        }
    }
}
