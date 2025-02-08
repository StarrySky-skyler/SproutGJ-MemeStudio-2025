using UnityEngine;

public class quitgame : MonoBehaviour
{
    public void QuitGame() 
    {

        //´æµµ£¿£¿


#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
