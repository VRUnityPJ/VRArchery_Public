using Cysharp.Threading.Tasks;
using Ranking.Scripts;
using Ranking.Scripts.DataBase;
//using Shinkan2025_Cooking.Scripts.Points;
using UnityEngine;

namespace Shinkan2025_Cooking.Ranking.Scripts
{
    /// <summary>
    /// VRShooting.Scripts.Ranking.RankingStorageの移植
    /// </summary>
    public class BoardUpdateManager : MonoBehaviour
    {
        [SerializeField] private BoardUpdater updater;
        private RankingData[] rankingDatas;
        private bool isMoving = true;
        //[SerializeField] RankingStorage storage;

        private void Awake()
        {
            rankingDatas = new RankingData[5]{null, null, null, null, null};
        }

        private void OnEnable()
        {
            isMoving = true;
            Flow();
        }

        private void OnDisable()
        {
            isMoving = false;
        }

        private async UniTask Flow()
        {
            if (!isMoving) return;
            await UniTask.Delay(1000);
            RankingData[] data = await PlayFabManager.GetLeaderboardAsync(5);
            //データが前回と同じなら更新しない
            if(IsEqual(data) == false)
                updater.UpdateRanking(data);
            
            for (int i = 0; i < 5; i++)
            {
                //Debug.Log(data[i].GetData<Point>().IntValue);
            }
            //合計10秒のインターバル
            await UniTask.Delay(9000);
            await Flow();
        }

        //Rankingdataを比べる
        private bool IsEqual(RankingData[] data)
        {
            if (rankingDatas[0] == null)
            {
                UpdateNowData(data);
                return false;
            }
            for (int i = 0; i < rankingDatas.Length; i++)
            {
                // if (data[i].GetData<Point>().IntValue != rankingDatas[i].GetData<Point>().IntValue)
                // {
                //     UpdateNowData(data);
                //     return false;
                // }
            }
            return true;
        }

        //Nowdataを更新する
        private void UpdateNowData(RankingData[] data)
        {
            rankingDatas = data;
        }
    }
}
