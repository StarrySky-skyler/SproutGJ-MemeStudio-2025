using UnityEngine;

namespace AnRan
{
    public class GameManager : Singleton<GameManager>
    {
        private static bool isDontDestroyOnLoad;

        [Header("µ±Ç°Ñ¡Ôñ´æµµ")]
        public UserData selectSaveData;

        void Start()
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


