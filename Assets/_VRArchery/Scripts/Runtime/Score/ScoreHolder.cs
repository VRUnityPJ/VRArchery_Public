using System;
using R3;
using RankingSystem.Scripts;
using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Score
{
    /// <summary>
    /// スコアを管理するクラス
    /// </summary>
    public sealed class ScoreHolder : IRankingDataElement<ScoreHolder>
    {
        /// <summary>
        /// スコアランク
        /// </summary>
        public Rank CurrentRank => _currentRank;

        private const int SRankThreshold = 17000;
        private const int ARankThreshold = 10000;
        private const int BRankThreshold = 6000;
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
        /// 加算するスコアを計算する
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="playerPos"></param>
        /// <returns></returns>
        public int CalculateAddScore(Vector3 targetPos, Vector3 allowPos,float speedBonus)
        {
            //現在プレイヤーと的の距離で得点を決定、それを生成されてからの時間、矢の命中座標、的の命中座標で計算
            var result = (targetPos - allowPos).sqrMagnitude*speedBonus;
            return (int)result;
        }

        /// <summary>
        /// 現在スコアに対するランクを取得
        /// </summary>
        public Rank GetRank()
        {
            if (_score.Value >= SRankThreshold)
                return Rank.弓聖;
            if (_score.Value >= ARankThreshold)
                return Rank.師範代;
            if (_score.Value >= BRankThreshold)
                return Rank.一人前;
            return Rank.半人前;
        }

        /// <summary>
        /// 次のランクに必要なスコアを返す
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        public static int GetNextRankScore(int score) =>
            score switch
            {
                >= SRankThreshold => 0,
                >= ARankThreshold => SRankThreshold - score,
                >= BRankThreshold => ARankThreshold - score,
                _ => BRankThreshold - score
            };
    }
}