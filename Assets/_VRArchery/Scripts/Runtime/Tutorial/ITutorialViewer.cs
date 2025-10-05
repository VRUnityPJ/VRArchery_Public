using Cysharp.Threading.Tasks;

namespace _VRArchery.Scripts.Runtime.Tutorial
{
    public interface ITutorialViewer
    {
        UniTask StartTutorialAsync();
    }
}