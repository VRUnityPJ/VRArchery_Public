using R3;
using UnityEngine;
using UnityEngine.Events;

namespace KeyBoard
{
    public class InputKeyCollector : MonoBehaviour,IKeyBoardEventTrigger
    {
        private ReactiveProperty<string> _TypedText = new ReactiveProperty<string>("");
        public ReadOnlyReactiveProperty<string> TypedText => _TypedText;
        private int _maxTextSize;
        private KeyBoardSetting _setting;
        
        //デバッグモードに移行する文字列
        [SerializeField] private string _debugModeText;

        /// <summary>
        /// 保存している文字列が0文字の時に文字を消そうとしたときのイベント
        /// </summary>
        [HideInInspector]
        public UnityEvent onDeleteEmptyText { get; private set; } = new UnityEvent();

        /// <summary>
        /// 保存している文字列が最大文字数を超えようとしたときのイベント
        /// </summary>
        [HideInInspector]
        public UnityEvent onOverFullSizeText { get; private set; } = new UnityEvent();

        /// <summary>
        /// Debug文字が入力されたとき
        /// </summary>
        [HideInInspector]
        public UnityEvent onTypedDebugText { get; set; } = new UnityEvent();

        public bool IsTextExit
        {
            get
            {
                return _TypedText.Value.Length > 0;
            }
        }
        
        [SerializeField] private KeyBoardViewer _viewer;
        private void Start()
        {
            //KeyBoardSettingを取得
            if(!TryGetComponent(out _setting))
                Debug.LogError("Settingが取得できません");
            
            //最大文字数を取得
            _maxTextSize = _setting.MaxTextSize;
            
            //TypeTextの更新をTextBoxに通知
            _TypedText.Subscribe(value =>
            {
                _viewer.UpdateTextBox(value);
            }).AddTo(this);
            
            //Debugモードの条件を満たしたときにイベントを発火
            _TypedText
                .Where(val => val == _debugModeText)
                .Subscribe(_ => onTypedDebugText?.Invoke())
                .AddTo(this);
        }
        
        public void AddText(string text)
        {
            //最大文字数を超えたらreturn
            if (_TypedText.Value.Length + text.Length > _maxTextSize)
            {
                onOverFullSizeText?.Invoke();
                return;
            }
            _TypedText.Value += text;
        }

        public void DelText()
        {
            var lastCharElement = _TypedText.Value.Length-1;
            //TypedTextが0文字ならreturn
            if (lastCharElement < 0)
            {
                onDeleteEmptyText?.Invoke();
                return;
            }

            //最後の一文字を削除
            _TypedText.Value = _TypedText.Value.Remove(lastCharElement);
        }
    }
}