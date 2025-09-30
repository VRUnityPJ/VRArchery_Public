using UnityEngine;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;

namespace KeyBoard
{
    public class DeleteButton : KeyBoardButton
    {
        private InputKeyCollector _collector;

        private void Start()
        {
            base.Start();

            _collector = MyMethod.GetComponentInParents<InputKeyCollector>(transform);
            
            SetColor();
            
            AddOnBeginClickListener((data) =>
            {
                _collector.DelText();
            });
        }
        
        protected override void SetColor()
        {
            defaultColor = _setting.DeleteButtonColor.defaultColor;
            clickedColor = _setting.DeleteButtonColor.clickedColor;
            pointedColor = _setting.DeleteButtonColor.pointedColor;
        }
    }
}