using System;
using System.IO;
using UnityEngine;
using Vector2Json.SaveSystem;
using AnRan;
public class SaveControl : MonoBehaviour
{
    public SaveCell[] cells;
    private readonly string _archiveFileNameFormatter = "archive";
    void Start()
    {
        AddSerializedJson.AddAllConverter();
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < 5; i++)
            {
                GameJamSaveSystem.SaveData(new UserData(_archiveFileNameFormatter + i,
                    DateTime.Now.ToString("yyyy/M/d-H:mm:ss"),
                    1,
                    i,
                    new Vector2(UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(1f, 2f)))
                    );
            }
        }
    }

    void LoadData()
    {
        string[] files = Directory.GetFiles(Application.streamingAssetsPath + "/GameJamSaveSystem", "*.JSON", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            UserData data = GameJamSaveSystem.LoadData(_archiveFileNameFormatter + i);

            cells[i].LoadData(data);
        }

    }
}
