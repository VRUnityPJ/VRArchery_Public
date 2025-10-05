using UnityEngine;

namespace RankingSystem.Scripts
{
    /// <summary>
    /// FirstSceneでPlayFabにログインするだけのクラス
    /// </summary>
    public class LogInManager : MonoBehaviour
    {
        void Start()
        {
            PlayFabManager.LogIn();
        }
    }
}
