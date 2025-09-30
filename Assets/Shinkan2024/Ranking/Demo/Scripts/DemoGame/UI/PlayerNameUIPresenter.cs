using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Ranking.Demo.Scripts.DemoGame
{
    public class PlayerNameUIPresenter : MonoBehaviour
    {
        [SerializeField] private PlayerNameHolder _model;
        [SerializeField] private PlayerNameUIViewer _viewer;

        private void Start()
        {
            _model.PlayerName.Subscribe(val => 
            {
                _viewer.UpdateText(val.StringValue);
            }).AddTo(this);
        }
    }
}