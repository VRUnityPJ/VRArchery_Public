using Cysharp.Threading.Tasks;
using Shinkan2025_Cooking.Ranking.Scripts;
using TMPro;
using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    /// <summary>
    /// アラート処理を管理するクラス
    /// </summary>
    public class Alart : MonoBehaviour
    {
        private TextMeshProUGUI _textMesh;
        private 
        void Start()
        {
            if(!TryGetComponent(out _textMesh))
                Debug.LogError("TextMeshProが取得できません");
            
            gameObject.SetActive(false);
            
            PlayFabManager.onCompleteLogin += OnCompleteLogin;
            PlayFabManager.onFailedLogin += OnFailedLogin;
            PlayFabManager.onCompleteRegister += OnCompleteRegister;
            PlayFabManager.onFailedRegister += OnFailedRegister;
            PlayFabManager.onCompleteGetLeaderBoard += OnCompleteGetLeaderBoard;
            PlayFabManager.onFailedGetLeaderBoard += OnFailedGetLeaderBoard;
        }
    
        private async void ShowAlart(string message,Color messageColor)
        {
            _textMesh.text = message;
            _textMesh.color = messageColor;
            
            //2秒間表示
            gameObject.SetActive(true);
            await UniTask.Delay(2000);
            gameObject.SetActive(false);
        }
        private async void OnCompleteLogin()
        {
            ShowAlart("Login Successful",Color.cyan);
        }
    
        private async void OnFailedLogin()
        {
            ShowAlart("Login Failed",Color.red);
        }
    
        private async void OnCompleteRegister()
        {
            ShowAlart("Register Successful",Color.cyan);
        }
    
        private async void OnFailedRegister()
        {
            ShowAlart("Register Failed",Color.red);
        }
    
        private async void OnCompleteGetLeaderBoard()
        {
            ShowAlart("Successfully Get Leader Board",Color.cyan);
        }
    
        private async void OnFailedGetLeaderBoard()
        {
            ShowAlart("Failed to Get Leader Board",Color.red);
        }
    }
}

