using _VRArchery.Scripts.Runtime.Stage;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using R3;

namespace _VRArchery.Scripts.Runtime.UI
{
    /// <summary>
    /// タイマーを表示させる用
    /// </summary>
    public class TimePresenter : MonoBehaviour
    {
        [SerializeField] private TimeController _timeController;
        [SerializeField] private TextMeshProUGUI _timeText;

        private void Start()
        {
            _timeController.LimitTimeSec
                .Subscribe(value =>
                {
                    _timeText.text = $"Time : {value:N1}";
                })
                .AddTo(destroyCancellationToken);
        }
    }
}