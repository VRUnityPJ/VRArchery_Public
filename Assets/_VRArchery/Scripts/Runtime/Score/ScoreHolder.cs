using R3;

namespace _VRArchery.Scripts.Runtime.Score
{
    /// <summary>
    /// スコアを管理するクラス
    /// </summary>
    public sealed class ScoreHolder
    {
        /// <summary>
        /// スコアランク
        /// </summary>
        public enum Rank {C, B, A,}

        private const int AThreshold = 100; // Aランクのしきい値
        private const int BThreshold = 50;  // Bランクのしきい値

        private readonly ReactiveProperty<int> _score = new();

        /// <summary>
        /// 現在のスコア
        /// </summary>
        public ReadOnlyReactiveProperty<int> Score => _score;

        /// <summary>
        /// スコアを初期化
        /// </summary>
        public void InitializeScore() => _score.Value = 0;

        /// <summary>
        /// 加算
        /// </summary>
        /// <param name="value"></param>
        public void AddScore(int value) => _score.Value += value;

        /// <summary>
        /// 減算
        /// </summary>
        /// <param name="value"></param>
        public void SubScore(int value)
        {
            if (_score.Value < value)
            {
                _score.Value = 0;
                return;
            }

            _score.Value -= value;
        }

        /// <summary>
        /// 現在スコアに対するランクを取得
        /// </summary>
        public Rank GetRank()
        {
            if (_score.Value >= AThreshold) return Rank.A;
            if (_score.Value >= BThreshold) return Rank.B;
            return Rank.C;
        }
    }
}