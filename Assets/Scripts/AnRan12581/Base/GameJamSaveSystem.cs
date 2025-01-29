using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;

public class GameJamSaveSystem
{
    public static Dictionary<string, UserData> usersData = new Dictionary<string, UserData>();

    public static char[] keyChars = { 'a', 'b', 'c', 'd', 'e' };

    public static string Encrypt(string data)
    {
        char[] dataChars = data.ToCharArray();
        for (int i = 0; i < dataChars.Length; i++)
        {
            char dataChar = dataChars[i];
            char keyChar = keyChars[i % keyChars.Length];
            char newChar = (char)(dataChar ^ keyChar);
            dataChars[i] = newChar;
        }
        return new string(dataChars);
    }

    public static string Decrypt(string data)
    {
        return Encrypt(data);
    }

    public static void SaveData(UserData userData)
    {

        if (!File.Exists(Application.streamingAssetsPath + "/GameJamSaveSystem"))
        {
            System.IO.Directory.CreateDirectory(Application.streamingAssetsPath + "/GameJamSaveSystem");
        }

        usersData[userData.Filename] = userData;

        string jsonData = JsonConvert.SerializeObject(userData);
#if UNITY_EDITOR

#else
     Encrypt(jsonData);
#endif

        File.WriteAllText(Application.streamingAssetsPath + string.Format("/GameJamSaveSystem/{0}.json", userData.Filename), jsonData);
    }

    public static UserData LoadData(string userName)
    {

        if (usersData.ContainsKey(userName))
        {
            return usersData[userName];
        }

        string path = Application.streamingAssetsPath + string.Format("/GameJamSaveSystem/{0}.json", userName);

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
#if UNITY_EDITOR

          
#else
    jsonData = Decrypt(jsonData);
#endif
            UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
            return userData;
        }
        else
        {
            return null;
        }
    }
}

public class UserData
{
    public string Filename;

    public int level;

    public Vector2 PlayerPosition;

    public UserData(string filename,int level, Vector2 playerPos)
    {
        Filename = filename;
        this.level = level;
        PlayerPosition = playerPos;
    }
}

public class Vector2Pos
{
    public float X;
    public float Y;

    public Vector2Pos(float X,float Y)
    {
        this.X = X;
        this.Y = Y;
    }

}
