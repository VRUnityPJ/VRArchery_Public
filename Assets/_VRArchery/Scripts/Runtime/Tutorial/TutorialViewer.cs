using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _VRArchery.Scripts.Runtime.Tutorial
{
    public class TutorialViewer : MonoBehaviour, ITutorialViewer
    {
        [SerializeField] private TextMeshProUGUI _tutorialText;

        [Space, SerializeField] private Button _yesButton;

        public async UniTask StartTutorialAsync()
        {

        }
    }
}