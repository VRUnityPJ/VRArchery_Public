using Shinkan2025_Cooking.Ranking.Scripts;
using UnityEngine;
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
