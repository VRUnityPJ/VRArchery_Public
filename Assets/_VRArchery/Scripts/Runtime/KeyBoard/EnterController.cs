using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace KeyBoard
{
    /// <summary>
    /// エンターボタンを押したときの処理を管理するクラス
    /// </summary>
    public class EnterController : ICompletable, IStartable
    {
        private readonly EnterButton _enter;
        private readonly InputKeyCollector _inputcol;

        public EnterController(EnterButton enter, InputKeyCollector inputcol)
        {
            _enter = enter;
            _inputcol = inputcol;
        }

        public void Start()
        {
            _enter.AddOnEndClickListener(_ => OnEnterButtonClicked());
        }

        private void OnEnterButtonClicked()
        {
            Debug.Log($"_enterbutton{_enter}, _inputcol{_inputcol}");
            if(!_inputcol.IsTextExit)
                return;
            Debug.Log("Enter");
        }

        /// <summary>
        /// ボタンが押されるまで待機する
        /// </summary>
        /// <param name="token"></param>
        public async UniTask OnComplete(CancellationToken token)
            => await _enter.OnClickAsync(cancellationToken: token);
    }
}