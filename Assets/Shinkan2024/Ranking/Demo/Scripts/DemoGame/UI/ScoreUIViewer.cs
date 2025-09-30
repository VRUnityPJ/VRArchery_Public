using TMPro;
using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
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