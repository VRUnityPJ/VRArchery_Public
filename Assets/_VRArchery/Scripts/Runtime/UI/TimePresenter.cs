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
            //通常のSubscribeの書き方だとクロージャが発生して余計なメモリ確保してしまう
            //ラムダ式の外側にある変数を参照することでヒープ領域にメモリを確保してGCの対象となってしまう
            //第一引数に参照したい変数をわたして
            //第二引数をstaticなラムダ式にすると大丈夫
            _timeController.LimitTimeSec
                .Subscribe(_timeText, static (value, timeText) =>
                {
                    timeText.text = $"Time : {value:N1}";
                })
                .AddTo(destroyCancellationToken);
        }
    }
}