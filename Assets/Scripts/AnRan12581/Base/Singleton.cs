
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    //private static bool isDontDestroyOnLoad;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {

        if (_instance != null && _instance != this)
        {
            //Debug.Log("µ¥ÀýÉ¾³ý:" + gameObject.name);
            //Destroy(gameObject); 
        }
        else
        {
            _instance = this as T;  
        }

        //if (!isDontDestroyOnLoad)
        //{
        //    isDontDestroyOnLoad = true;
        //    DontDestroyOnLoad(gameObject);

        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
