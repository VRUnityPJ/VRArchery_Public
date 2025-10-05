using UnityEngine;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

namespace KeyBoard
{
    public class KeyButton : KeyBoardButton
    {
        [SerializeField] private string myChar;
        private InputKeyCollector _collector;
        private void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            base.Start();
            //InputKeyCollectorを取得
            _collector = MyMethod.GetComponentInParents<InputKeyCollector>(transform);
            //色の設定
            SetColor();

            //テキストを自身のアルファベットに設定
            _textMesh.text = myChar;
            
            AddOnBeginClickListener(data => SendChar());
        }

        private void SendChar()
        {
            _collector.AddText(myChar);
        }

        protected override void SetColor()
        {
            defaultColor = _setting.KeyButtonColor.defaultColor;
            clickedColor = _setting.KeyButtonColor.clickedColor;
            pointedColor = _setting.KeyButtonColor.pointedColor;
        }
    }
}