using R3;
using UnityEngine.Events;

namespace KeyBoard
{
    public interface IKeyBoardEventTrigger
    {
        public ReadOnlyReactiveProperty<string> TypedText { get; }
        
        /// <summary>
        /// 保存している文字列が0文字の時に文字を消そうとしたときのイベント
        /// </summary>
        public UnityEvent onDeleteEmptyText { get; }
        /// <summary>
        /// 保存している文字列が最大文字数を超えようとしたときのイベント
        /// </summary>
        public UnityEvent onOverFullSizeText{ get; }
        
        /// <summary>
        /// Debug文字が入力されたとき
        /// </summary>
        public UnityEvent onTypedDebugText { get;}
    }
}