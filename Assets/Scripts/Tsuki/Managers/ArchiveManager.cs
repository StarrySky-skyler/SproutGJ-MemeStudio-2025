// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/30 21:01
// @version: 1.0
// @description: 存档管理器单例
// ********************************************************************************

using System;
using System.Collections.Generic;
using AnRan;
using Tsuki.MVC.Models.Player;
using UnityEngine;

namespace Tsuki.Managers
{
    public class ArchiveManager : Singleton<ArchiveManager>
    {
        [Header("存档槽位总数")] public int archiveCount;

        private List<UserData> _userDataList;
        private PlayerModel _playerModel;
        private readonly string _archiveFileNameFormatter = "archive";

        protected override void Awake()
        {
            base.Awake();
            _userDataList = new List<UserData>();
            ReadAllArchive();
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
        }

        /// <summary>
        /// 保存当前游戏存档
        /// <param name="archiveIndex">存档槽位索引，从0开始</param>
        /// </summary>
        public void SaveCurrentArchive(int archiveIndex)
        {
            if (archiveIndex < 0 || archiveIndex >= archiveCount)
            {
                Debug.LogError("存档槽位索引越界");
                return;
            }

            _userDataList[archiveIndex] = new UserData(_archiveFileNameFormatter + archiveIndex,
                GameManager.Instance.CurrentLevel,
                _playerModel.CurrentPos);
        }

        /// <summary>
        /// 读取所有存档
        /// </summary>
        private void ReadAllArchive()
        {
            _userDataList.Clear();
            for (int i = 0; i < archiveCount; i++)
            {
                UserData loadedData = GameJamSaveSystem.LoadData(_archiveFileNameFormatter + i);
                if (loadedData == null)
                {
                    Debug.LogWarning($"存档文件{_archiveFileNameFormatter + i}不存在，跳过读取");
                    continue;
                }
                _userDataList.Add(loadedData);
            }
        }

        /// <summary>
        /// 加载存档
        /// </summary>
        /// <param name="archiveIndex">存档槽位索引，从0开始</param>
        public void LoadArchive(int archiveIndex)
        {
            // TODO: 加载具体关卡
        }
    }
}
