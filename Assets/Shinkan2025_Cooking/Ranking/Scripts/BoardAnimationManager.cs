using DG.Tweening;
using Ranking.Scripts;
using Ranking.Scripts.DataBase;
//using Shinkan2025_Cooking.Scripts.Points;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shinkan2025_Cooking.Ranking.Scripts
{
    public class BoardAnimationManager : MonoBehaviour
    {
        [SerializeField] private GameObject NameParent;
        [SerializeField] private GameObject ScoreParent;
        [SerializeField] private Button[] buttons;
        private TextMeshProUGUI[] nameTexts;
        private TextMeshProUGUI[] scoreTexts;
        private void Start()
        {
            nameTexts = NameParent.GetComponentsInChildren<TextMeshProUGUI>();
            scoreTexts = ScoreParent.GetComponentsInChildren<TextMeshProUGUI>();
            
            if(nameTexts.Length != scoreTexts.Length)
                Debug.LogError("プレイヤー名とスコアのテキストの数が一致していません");
        }

        public void FadeOut()
        {
            foreach (var btn in buttons)
            {
                btn.interactable = false;
            }
            
            foreach (var txt in nameTexts)
            {
                txt.DOFade(0, 0.5f);
            }

            foreach (var txt in scoreTexts)
            {
                txt.DOFade(0, 0.5f);
            }
            
        }

        public void FadeIn()
        {
            var seq = DOTween.Sequence().Pause();
            
            for (int i = 0; i < nameTexts.Length; i++)
            {
                seq.Append(nameTexts[i].DOFade(1, 0.4f));
                seq.Append(scoreTexts[i].DOFade(1, 0.4f));
                seq.AppendInterval(0.2f); 
            }

            seq.AppendCallback(() =>
            {
                foreach (var btn in buttons)
                {
                    btn.interactable = true;
                }
            });
            seq.Restart();
        }

        public void UpdateNameText(RankingData[] records)
        {
            if(records.Length != nameTexts.Length)
                Debug.LogError("取得したランキングの数がリーダーボードと一致していません");
            
            for (int i = 0; i < nameTexts.Length; i++)
            {
                //Point point = records[i].GetData<Point>();
                PlayerName playerName = records[i].GetData<PlayerName>();
                Debug.Log(playerName.StringValue);
                nameTexts[i].text = playerName.StringValue;
                Debug.Log("nameText = " + nameTexts[i].text);
                //scoreTexts[i].text = point.IntValue.ToString();
                Debug.Log("ランキングボードを更新しました");
            }
        }
    }
}