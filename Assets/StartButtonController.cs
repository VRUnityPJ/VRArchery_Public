using UnityEngine;
using UnityEngine.UI;
using TMPro; // ← これを追加
using DG.Tweening;
using System.Collections; // ← これが必要！

public class StartButtonController : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI countdownText;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        countdownText.gameObject.SetActive(false); // 最初は非表示
    }

    void OnStartButtonClicked()
    {
        // アニメーション：ふわっと大きくなる
        startButton.transform.DOScale(1.2f, 0.3f)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                // カウントダウンテキストの位置をボタンと同じにする
                countdownText.rectTransform.position = startButton.transform.position;

                // アニメーションが終わったらボタンを非表示にする
                startButton.gameObject.SetActive(false);

                // カウントダウン開始
                StartCoroutine(StartCountdown());
            });

        // 色変更（例：赤に変化） → これはすぐに始めてOK
        Color targetColor = Color.green;
        startButton.image.DOColor(targetColor, 0.5f);
    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        string[] countdown = { "3", "2", "1", "START" };
        foreach (string count in countdown)
        {
            countdownText.text = count;


            // スケールをリセットしてからアニメーション
            countdownText.rectTransform.localScale = Vector3.one * 0.5f; // 小さくして
            countdownText.rectTransform.DOScale(1f, 0.3f); // ふわっと大きく

            yield return new WaitForSeconds(1f);
        }

        countdownText.gameObject.SetActive(false);

        // ゲーム開始処理をここに書く
        Debug.Log("ゲームスタート！");
    }
}