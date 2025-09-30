using System;
using R3;

namespace Ranking.Scripts.Interface
{
    /// <summary>
    /// ランキングに登録する要素クラスが継承するインターフェース
    /// ～ 注意 ～ RankingDataクラス作成時に必要なため必ずデフォルトコンストラクタを定義すること
    /// </summary>
    public interface IRankingDataElement<T>{}
}
