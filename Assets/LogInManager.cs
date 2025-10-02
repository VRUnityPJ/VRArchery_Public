using RankingSystem.Scripts;
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
