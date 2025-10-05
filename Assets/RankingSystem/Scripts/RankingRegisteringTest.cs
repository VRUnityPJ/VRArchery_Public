using Ranking.Scripts;
using RankingSystem.Scripts;
using UnityEngine;

public class RankingRegisteringTest : MonoBehaviour
{
    [SerializeField]
    private RankingStorage _rankingStorage;

    void Start()
    {
        _rankingStorage.Register();
        Debug.Log("Registered");
    }
}
