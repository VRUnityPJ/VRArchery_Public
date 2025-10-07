using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _VRArchery.Scripts.Runtime.Tutorial
{
    public class TutorialPresenter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tutorialText;
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private Button _okButton;

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Init()
        {
            _tutorialText.transform.localScale = Vector3.zero;
            _yesButton.transform.localScale = Vector3.zero;
            _noButton.transform.localScale = Vector3.zero;
            _okButton.transform.localScale = Vector3.zero;
        }

        /// <summary>
        /// チュートリアルを実行するかどうかをユーザーに問いかけ、その結果を返す
        /// </summary>
        /// <param name="ct">CancellationToken</param>
        /// <returns>Yesボタンが押されたらtrue、Noボタンが押されたらfalse</returns>
        public async UniTask<bool> TryTutorialAsync(CancellationToken ct)
        {
            _tutorialText.text = "チュートリアルを開始しますか？";
            _tutorialText.transform.localScale = Vector3.one;

            await DOTween.Sequence()
                .Append(_yesButton.transform.DOScale(Vector3.one, 0.3f))
                .Append(_noButton.transform.DOScale(Vector3.one, 0.3f))
                .ToUniTask(cancellationToken: ct);

            var result = await UniTask.WhenAny(
                _yesButton.OnClickAsync(ct),
                _noButton.OnClickAsync(ct)
                );

            await DOTween.Sequence()
                .Append(_yesButton.transform.DOScale(Vector3.zero, 0.3f))
                .Append(_noButton.transform.DOScale(Vector3.zero, 0.3f))
                .Append(_tutorialText.transform.DOScale(Vector3.zero, 0.3f))
                .ToUniTask(cancellationToken: ct);

            // yesボタンなら0(true)を返す
            return result == 0;
        }

        /// <summary>
        /// チュートリアルを開始する
        /// </summary>
        /// <param name="ct"></param>
        public async UniTask StartTutorialAsync(CancellationToken ct)
        {
            _tutorialText.text = "今からチュートリアルを開始するよ";
            await _tutorialText.transform.DOScale(Vector3.one, 0.3f).ToUniTask(cancellationToken: ct);

            _okButton.transform.DOScale(Vector3.one, 0.3f)
                .ToUniTask(cancellationToken: ct)
                .Forget();

            await _okButton.OnClickAsync(ct);
            _tutorialText.text = "左手のTriggerボタンを押すと弓を掴めるよ";

            await _okButton.OnClickAsync(ct);
            _tutorialText.text = "右手のTriggerで弓を発射できるよ";

            await _okButton.OnClickAsync(ct);
            _tutorialText.text = "これでチュートリアルを終了するよ";

            var seq = DOTween.Sequence();

            await seq
                .Append(_okButton.transform.DOScale(Vector3.zero, 0.3f))
                .Append(_tutorialText.transform.DOScale(Vector3.zero, 0.3f))
                .ToUniTask(cancellationToken: ct);
        }
    }
}