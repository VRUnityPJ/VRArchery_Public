using System;
using Ranking.Scripts;
using Ranking.Scripts.DataBase;
using TMPro;
using UnityEngine;

namespace Ranking.Demo
{
    /// <summary>
    /// RankingDemoシーン内で使うクラス
    /// ランキングボードを表示する
    /// </summary>
    public class RankingTable : MonoBehaviour
    {
        private TextMeshProUGUI textMesh;
        private void Start()
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
            
            if(!textMesh)
                Debug.LogError("TextMeshProGUIが取得できてません");
        }

        public void Show(RankingData data , int rank)
        {
            //Playfabの場合
            //サーバーの順位は0からなので+1
            rank++;
            
            //表示
            textMesh.text = $"No.{rank.ToString()}  {data.GetData<PlayerName>().StringValue}  {data.GetData<Score>().IntValue.ToString()}";
        }
    }
}