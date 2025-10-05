using UnityEngine;
using DG.Tweening;
using PlayFab.Internal;
using TMPro;
using UnityEngine.UI;

namespace KeyBoard
{
    /// <summary>
    /// キーボードに使用するボタンのクラス
    /// </summary>
    public class KeyBoardButton : VRButton
    {
        protected KeyBoardSetting _setting;
        
        protected Color pointedColor;
        protected Color clickedColor;
        protected Color defaultColor;
        protected Image _frame;
        protected TextMeshProUGUI _textMesh;

        protected void Start()
        {
            _frame = GetComponentInChildren<Image>();
            if(!_frame)
                Debug.LogError("Frameが取得できません");
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
            if(!_textMesh)
                Debug.LogError("Textが取得できません");
            _setting = MyMethod.GetComponentInParents<KeyBoardSetting>(transform);
            
            //色を変えるイベントの登録
            AddOnBeginClickListener((data) => ChangeColorToClicked());
            AddOnTouchListener((data) => ChangeColorToPointed());
            AddOnExitListener((data) => ChangeColorToDefault());
            AddOnEndClickListener((data) => ChangeColorToPointed());
        }
        //色の設定
        protected virtual void SetColor(){}
        public void ChangeColorToPointed()
        {
            _frame.color = pointedColor;
            _textMesh.color = pointedColor;
        }

        public void ChangeColorToClicked()
        {
            _frame.color = clickedColor;
            _textMesh.color = clickedColor;
        }

        public void ChangeColorToDefault()
        {
            _frame.color = defaultColor;
            _textMesh.color = defaultColor;
        }

        public void DoFade(float endValue, float duration)
        {
            _frame.DOFade(endValue, duration);
            _textMesh.DOFade(endValue, duration);
        }

        public virtual void Clicked(){}
    }
}