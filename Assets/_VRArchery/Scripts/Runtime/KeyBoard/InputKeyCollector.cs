using System.Text;
using R3;
using UnityEngine;
using UnityEngine.Events;

namespace KeyBoard
{
    public class InputKeyCollector : MonoBehaviour,IKeyBoardEventTrigger
    {
        private readonly ReactiveProperty<string> _typedText = new ReactiveProperty<string>("");
        public ReadOnlyReactiveProperty<string> TypedText => _typedText;
        private int _maxTextSize;
        private KeyBoardSetting _setting;

        //デバッグモードに移行する文字列
        [SerializeField] private string _debugModeText;

        /// <summary>
        /// 保存している文字列が0文字の時に文字を消そうとしたときのイベント
        /// </summary>
        public UnityEvent OnDeleteEmptyText { get; } = new();

        /// <summary>
        /// 保存している文字列が最大文字数を超えようとしたときのイベント
        /// </summary>
        public UnityEvent OnOverFullSizeText { get; } = new();

        /// <summary>
        /// Debug文字が入力されたとき
        /// </summary>
        public UnityEvent OnTypedDebugText { get; } = new();

        public bool IsTextExit => _typedText.Value.Length > 0;

        [SerializeField] private KeyBoardViewer _viewer;
        private void Start()
        {
            //KeyBoardSettingを取得
            if(!TryGetComponent(out _setting))
                Debug.LogError("Settingが取得できません");

            //最大文字数を取得
            _maxTextSize = _setting.MaxTextSize;

            //TypeTextの更新をTextBoxに通知
            _typedText.Subscribe(value =>
            {
                _viewer.UpdateTextBox(value);

            }).AddTo(this);

            //Debugモードの条件を満たしたときにイベントを発火
            _typedText
                .Where(val => val == _debugModeText)
                .Subscribe(_ => OnTypedDebugText?.Invoke())
                .AddTo(this);
        }

        public void AddText(string text)
        {
            //最大文字数を超えたらreturn
            if (_typedText.Value.Length + text.Length > _maxTextSize)
            {
                OnOverFullSizeText?.Invoke();
                return;
            }
            _typedText.Value += text;
        }

        public void DelText()
        {
            var lastCharElement = _typedText.Value.Length-1;
            //TypedTextが0文字ならreturn
            if (lastCharElement < 0)
            {
                OnDeleteEmptyText?.Invoke();
                return;
            }

            //最後の一文字を削除
            _typedText.Value = _typedText.Value.Remove(lastCharElement);
        }
    }
}