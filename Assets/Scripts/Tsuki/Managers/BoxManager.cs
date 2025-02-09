// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/29 23:01
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Base;
using Tsuki.Entities.Box.Base;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class BoxManager : Singleton<BoxManager>
    {
        public UnityEvent<bool> onWinChanged = new();
        public UnityEvent onBoxCorrectAdded = new();
        public UnityEvent onBoxCorrectRemoved = new();
        private int _boxCorrectCount;
        private int _boxCount;

        private bool _win;

        public bool Win
        {
            get => _win;
            private set
            {
                if (_win == value) return;
                _win = value;
                DebugYumihoshi.Log<BoxManager>("箱子",
                    _win ? "所有箱子已归位" : "所有箱子未归位");
                onWinChanged?.Invoke(_win);
            }
        }

        private void Start()
        {
            ResetBoxCount();
        }

        private void OnEnable()
        {
            // 注册事件
            SceneManager.sceneLoaded += ResetBoxCount;
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged.AddListener(
                RepeatAllLastPos);
        }

        private void OnDisable()
        {
            // 注销事件
            SceneManager.sceneLoaded -= ResetBoxCount;
            ModelsManager.Instance.PlayerMod.onMoveStatusChanged
                .RemoveListener(RepeatAllLastPos);
        }

        private void ResetBoxCount(Scene scene, LoadSceneMode mode)
        {
            ResetBoxCount();
        }

        private void ResetBoxCount()
        {
            _boxCorrectCount = 0;
            _win = false;
            _boxCount = GameObject.FindGameObjectsWithTag("Box").Length;
        }

        /// <summary>
        ///     增加正确的箱子
        /// </summary>
        public void AddCorrectBox()
        {
            _boxCorrectCount = Mathf.Min(_boxCorrectCount + 1, _boxCount);
            if (!GameManager.Instance.AllowLoadGame) return;
            onBoxCorrectAdded?.Invoke();
            DebugYumihoshi.Log<BoxManager>("箱子计分",
                $"增加正确的箱子，当前正确的箱子数量：{_boxCorrectCount}，总箱子数量：{_boxCount}");
            CheckWin();
        }

        /// <summary>
        ///     减少正确的箱子
        /// </summary>
        public void RemoveCorrectBox()
        {
            _boxCorrectCount = Mathf.Max(_boxCorrectCount - 1, 0);
            onBoxCorrectRemoved?.Invoke();
            DebugYumihoshi.Log<BoxManager>("箱子计分",
                $"增加正确的箱子，当前正确的箱子数量：{_boxCorrectCount}，总箱子数量：{_boxCount}");
        }

        private void CheckWin()
        {
            Win = _boxCorrectCount == _boxCount;
        }

        /// <summary>
        ///     重复所有箱子的最后位置
        /// </summary>
        /// <param name="moveStatus"></param>
        private void RepeatAllLastPos(bool moveStatus)
        {
            if (!moveStatus) return;
            GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
            foreach (GameObject box in boxes)
                box.GetComponent<BaseObj>().RepeatPos();
            GameObject[] weeders = GameObject.FindGameObjectsWithTag("Weeders");
            foreach (GameObject weeds in weeders)
                weeds.GetComponent<BaseObj>().RepeatPos();
        }
    }
}
