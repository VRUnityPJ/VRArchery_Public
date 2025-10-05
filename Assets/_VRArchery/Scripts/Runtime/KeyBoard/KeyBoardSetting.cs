using System;
using UnityEngine;

namespace KeyBoard
{
    public class KeyBoardSetting : MonoBehaviour
    {
        /// <summary>
        /// キーボードに入力できる最大文字数
        /// </summary>
        [SerializeField] private int _maxTextSize;
        public int MaxTextSize => _maxTextSize;
        
        /// <summary>
        /// KeyButtonの色設定
        /// </summary>
        [SerializeField] private ButtonColors _keyButtonColor;
        public ButtonColors KeyButtonColor => _keyButtonColor;
        
        /// <summary>
        /// DeleteButtonの色設定
        /// </summary>
        [SerializeField] private ButtonColors _deleteButtonColor;
        public ButtonColors DeleteButtonColor => _deleteButtonColor;
        
        /// <summary>
        /// EnterButtonの色設定
        /// </summary>
        [SerializeField] private ButtonColors _enterButtonColor;
        public ButtonColors EnterButtonColor => _enterButtonColor;

        /// <summary>
        /// 通常クリック音
        /// </summary>
        [SerializeField] private AudioClip _keyClickSE;
        public AudioClip KeyClickSE => _keyClickSE;
        
        /// <summary>
        /// Deleteクリック音
        /// </summary>
        [SerializeField] private AudioClip _deleteClickSE;
        public AudioClip DeleteClickSE => _deleteClickSE;
        
        /// <summary>
        /// Enterクリック音
        /// </summary>
        [SerializeField] private AudioClip _enterClickSE;
        public AudioClip EnterClickSE => _enterClickSE;
        
    }
    
    /// <summary>
    /// ボタンに設定する色
    /// </summary>
    [Serializable]
    public class ButtonColors
    {
        /// <summary>
        /// 通常の色
        /// </summary>
        public Color defaultColor;
        /// <summary>
        /// カーソルが重なっているときの色
        /// </summary>
        public Color pointedColor;
        /// <summary>
        /// クリックされているときの色
        /// </summary>
        public Color clickedColor;
    }
}