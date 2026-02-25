using System;
using R3;
using RankingSystem.Scripts;
using UnityEngine;
using _VRArchery.Scripts.Utility;
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
        // 最初の的の中心からの距離のしきい値
        private const float FirstTargetDistance = 2f;
        private const float SecondTargetDistance = 5f;
        // 最初の的の中心からの距離に応じた得点
        private const int FirstTargetScore = 200;
        private const int SecondTargetScore = 150;
        private const int NormalTargetScore = 100;
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
        /// <param name="targetPos"> 的の座標 </param>
        /// <param name="arrowPos"> 矢の座標 </param>
        /// <param name="speedBonus"> 速度ボーナス係数 </param>
        /// <returns> 加算するスコア </returns>
        public int CalculateAddScore(Vector3 targetPos, Vector3 arrowPos, float speedBonus)
        {
            float result = 0;
            //現在プレイヤーと的の距離で得点を決定、それを生成されてからの時間、矢の命中座標、的の命中座標で計算
            var distance = (targetPos - arrowPos).sqrMagnitude;
            if (distance < FirstTargetDistance)
            {
                CustomDebug.Log("ドンピシャ");
                result = FirstTargetScore;
            }
            else if (distance < SecondTargetDistance)
            {
                CustomDebug.Log("まあまあ真ん中");
                result = SecondTargetScore;
            }
            else
            {
                result = NormalTargetScore;
            }
            result *= speedBonus;
            CustomDebug.Log("ボーナス" + speedBonus + "ポイント" + result + "中心からの距離" + distance);
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