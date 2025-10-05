using TMPro;
using UnityEngine;

namespace RankingSystem.Scripts
{
    public class ScoreUIViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textGUI;
        public void UpdateText(int num)
        {
            _textGUI.text = $"Score:{num}";
        }
    }
}