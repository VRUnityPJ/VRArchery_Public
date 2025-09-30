using Ranking.Scripts.Interface;

namespace Ranking.Scripts
{
    public class Score : IRankingDataElement<Score>
    {
        private readonly int _intValue;
        public int IntValue => _intValue;
        
        /// <summary>
        /// デフォルトコンストラクタ
        /// ランキングデータの生成に必要
        /// </summary>
        public Score()
        {
            _intValue = 0;
        }
        public Score(int initialNum)
        {
            _intValue = initialNum;
        }
        
        /// <summary>
        /// スコアを加算する
        /// score = score.Add(new Score(100)) とする
        /// </summary>
        public Score Add(Score other)
        {
            var n = _intValue + other.IntValue;
            return new Score(n);
        }

        public Score Subtract(Score other)
        {
            var n = _intValue - other.IntValue;
            return new Score(n);
        }
    } 
}