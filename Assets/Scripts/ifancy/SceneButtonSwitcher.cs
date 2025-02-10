using Tsuki.Entities.ScreenMask;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonSwitcher : MonoBehaviour
{
    public string SceneName;

    public void SwitchScene()
    {
        if (SceneManager.GetActiveScene().name == "Menu" && SceneName == "Menu")
            Hide();
        else
            GameObject.FindWithTag("ScreenMask").GetComponent<ScreenMask>()
                .FadeIn(
                    () => { SceneManager.LoadScene(SceneName); });
    }

    public void Hide()
    {
        gameObject.transform.parent.parent.gameObject.SetActive(false);
    }
}
