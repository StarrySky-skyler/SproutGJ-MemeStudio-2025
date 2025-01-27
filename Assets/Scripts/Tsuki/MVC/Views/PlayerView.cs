// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 19:01
// @version: 1.0
// @description:
// ********************************************************************************

using Tsuki.MVC.Models;
using UnityEngine;

namespace Tsuki.MVC.Views
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerModel _playerModel;

        private void Awake()
        {
            // MVC 初始化
            _playerModel = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
        }
    }
}
