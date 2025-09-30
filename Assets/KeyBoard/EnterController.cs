using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeyBoard
{
    /// <summary>
    /// エンターボタンを押したときの処理を管理するクラス
    /// </summary>
    public class EnterController : MonoBehaviour
    {
        [SerializeField, Scene] private string _gameScene;
        [SerializeField] private EnterButton _enter;
        [SerializeField] private InputKeyCollector _inputcol;
        
        private void Start()
        {
            _enter.AddOnEndClickListener(_ => OnEnterButtonClicked());
        }
        private void OnEnterButtonClicked()
        {
            if(!_inputcol.IsTextExit)
                return;
            Debug.Log("Enter");
            SceneManager.LoadScene(_gameScene);
        }
    }
}