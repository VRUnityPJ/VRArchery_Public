using UnityEngine;
using UnityEngine.UI;
using TMPro; // ← 追加！
using System.Collections;
public class StartGame : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI countdownText; // ← Text → TMP_Text に変更！

    void Start()
    {
        countdownText.gameObject.SetActive(false);
        startButton.onClick.AddListener(() => StartCoroutine(Countdown()));
    }

    IEnumerator Countdown()
    {
        startButton.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        string[] countdown = { "3", "2", "1", "Start!!" };
        foreach (string count in countdown)
        {
            countdownText.text = count;
            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);

        // ゲーム開始処理をここに書く
    }
}