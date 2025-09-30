using UnityEngine;
using DG.Tweening;
using System;

namespace KeyBoard
{
    public class EnterButton : KeyBoardButton
    {
        protected void Start()
        {
            base.Start();
            SetColor();
        }
        
        protected override void SetColor()
        {
            defaultColor = _setting.EnterButtonColor.defaultColor;
            clickedColor = _setting.EnterButtonColor.clickedColor;
            pointedColor = _setting.EnterButtonColor.pointedColor;
        }
    }
}