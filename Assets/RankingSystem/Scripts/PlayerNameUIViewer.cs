using TMPro;
using UnityEngine;

namespace RankingSystem.Scripts
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