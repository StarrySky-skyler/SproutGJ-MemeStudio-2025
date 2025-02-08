using UnityEditor;
using UnityEngine;

public class quitgame : MonoBehaviour
{
    public void QuitGame()
    {
        //�浵����


#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
