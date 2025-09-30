using TMPro;
using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    public class PlayerNameUIViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGUI;
        public void UpdateText(string name)
        {
            textGUI.text = name;
        }
    }
}