using UnityEngine;

namespace AnRan
{
    public class GameManager : Singleton<GameManager>
    {
        private static bool isDontDestroyOnLoad;

        [Header("��ǰѡ��浵")] public UserData selectSaveData;

        private void Start()
        {
            if (!isDontDestroyOnLoad)
            {
                isDontDestroyOnLoad = true;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
