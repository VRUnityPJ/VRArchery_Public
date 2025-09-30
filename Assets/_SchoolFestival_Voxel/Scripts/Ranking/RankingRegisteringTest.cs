using Ranking.Scripts;
using UnityEngine;

public class RankingRegisteringTest : MonoBehaviour
{
    [SerializeField]
    private RankingStorage _rankingStorage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rankingStorage.Register();
            Debug.Log("Registered");
        }
    }
}
