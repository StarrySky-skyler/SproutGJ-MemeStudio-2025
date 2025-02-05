using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButtonSwitcher : MonoBehaviour
{
    public string SceneName;
    public void SwitchScene()
    {
        SceneManager.LoadScene(SceneName);
    }

}