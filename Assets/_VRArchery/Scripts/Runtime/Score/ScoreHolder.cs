using R3;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Score
{
    /// <summary>
    /// スコアを管理するクラス
    /// </summary>
    public sealed class ScoreHolder:MonoBehaviour
    {
        /// <summary>
        /// スコアランク
        /// </summary>
        public Rank CurrentRank => _currentRank;

        private const int ARankThreshold = 100;
        private const int BRankThreshold = 50;
        private Rank _currentRank;

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
            if (_score.Value >= ARankThreshold)
                return Rank.A;
            if (_score.Value >= BRankThreshold)
                return Rank.B;
            return Rank.C;
        }
    }
}