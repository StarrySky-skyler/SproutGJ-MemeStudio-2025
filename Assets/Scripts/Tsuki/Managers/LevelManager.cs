// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/02 15:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        private const string LEVEL_NAME_FORMATTER = "Level";
        private const string LEVEL_NAME_LATTER = "Scene";
        private const string PATTERN = @"Level(\d+)Scene";
        private UserData _loadData;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += PrintDebug;
            // TODO: 后期胜利应先弹出胜利UI，再加载下一关
            BoxManager.Instance.onWinChanged.AddListener(LoadNextLevel);
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= PrintDebug;
            BoxManager.Instance.onWinChanged.RemoveListener(LoadNextLevel);
        }

        private void PrintDebug(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("当前关卡数：" + GetCurrentLevel());
        }

        /// <summary>
        /// 获取当前关卡数
        /// </summary>
        /// <returns></returns>
        public int GetCurrentLevel()
        {
            // 使用正则表达式匹配场景名称中的数字部分
            string sceneName = SceneManager.GetActiveScene().name;
            Match match = Regex.Match(sceneName, PATTERN);
            if (match.Success) return int.Parse(match.Groups[1].Value);

            // 如果匹配失败，则返回-1
            Debug.LogError("获取当前关卡数错误，当前场景名称格式不正确");
            return -1;
        }

        /// <summary>
        /// 根据存档数据加载关卡
        /// </summary>
        /// <param name="userData"></param>
        public void LoadLevel(UserData userData)
        {
            _loadData = userData;
            SceneManager.sceneLoaded += SetObjsPos;
            SceneManager.LoadScene(LEVEL_NAME_FORMATTER + userData.level +
                                   LEVEL_NAME_LATTER);
        }

        private void SetObjsPos(Scene scene, LoadSceneMode mode)
        {
            GameObject.FindWithTag("Player").transform.position =
                _loadData.pos;
            SceneManager.sceneLoaded -= SetObjsPos;
            Debug.Log("玩家位置为：" + _loadData.pos);
        }

        /// <summary>
        /// 加载下一关
        /// </summary>
        /// <param name="win"></param>
        private void LoadNextLevel(bool win)
        {
            if (!win) return;
            // 如果是最后一关，则不加载下一关
            if (GetCurrentLevel() ==
                ModelsManager.Instance.GameMod.maxLevel - 1)
                BoxManager.Instance.onWinChanged.RemoveListener(LoadNextLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +
                                   1);
        }
    }
}
