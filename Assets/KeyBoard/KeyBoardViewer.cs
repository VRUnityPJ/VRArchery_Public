using TMPro;
using UnityEngine;

namespace KeyBoard
{
    public class KeyBoardViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        /// <summary>
        /// テキストボックスのテキストを更新
        /// </summary>
        public void UpdateTextBox(string inputText)
        {
            _textMesh.text = inputText;
        }
    }
}