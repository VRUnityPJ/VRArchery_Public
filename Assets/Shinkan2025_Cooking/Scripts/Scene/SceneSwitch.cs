using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [Scene, Required] public string _sceneName;

    public void SwitchScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
