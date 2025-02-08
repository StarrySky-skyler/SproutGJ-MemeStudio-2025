// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 21:01
// @version: 1.0
// @description: 存档管理器单例
// *****************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2Json.SaveSystem;

namespace Tsuki.Managers
{
    public class ArchiveManager : Singleton<ArchiveManager>
    {
        [SerializeField] private List<UserData> _userDataList;

        private readonly string _archiveFileNameFormatter = "archive";

        protected override void Awake()
        {
            base.Awake();
            AddSerializedJson.AddAllConverter();
            _userDataList = new List<UserData>();
        }

        private void Start()
        {
            ReadAllArchive();
        }

        /// <summary>
        ///     保存当前游戏存档
        ///     <param name="archiveIndex">存档槽位索引，从0开始</param>
        /// </summary>
        public void SaveCurrentArchive(int archiveIndex = 0)
        {
            if (archiveIndex < 0 || archiveIndex >=
                ModelsManager.Instance.GameMod.archiveCount)
            {
                Debug.LogError("存档槽位索引越界");
                return;
            }

            _userDataList[archiveIndex] = new UserData(
                _archiveFileNameFormatter + archiveIndex,
                DateTime.Now.ToString("yyyy/M/d-H:mm:ss"),
                LevelManager.Instance.GetCurrentLevel(),
                2f,
                ModelsManager.Instance.PlayerMod.CurrentPos
            );
        }

        /// <summary>
        ///     读取所有存档
        /// </summary>
        private void ReadAllArchive()
        {
            _userDataList.Clear();
            for (int i = 0;
                 i < ModelsManager.Instance.GameMod.archiveCount;
                 i++)
            {
                UserData loadedData =
                    GameJamSaveSystem.LoadData(_archiveFileNameFormatter + i);
                if (loadedData == null)
                {
                    Debug.LogWarning(
                        $"存档文件{_archiveFileNameFormatter + i}不存在，跳过读取");
                    continue;
                }

                _userDataList.Add(loadedData);
            }
        }

        /// <summary>
        ///     加载存档
        /// </summary>
        /// <param name="archiveIndex">存档槽位索引，从0开始</param>
        public void LoadArchive(int archiveIndex)
        {
            LevelManager.Instance.LoadLevel(_userDataList[archiveIndex]);
        }
    }
}
