// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/05 15:02
// @version: 1.0
// @description:
// *****************************************************************************

using Tsuki.Base;
using Tsuki.MVC.Models.Game;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tsuki.Managers
{
    public class ModelsManager : Singleton<ModelsManager>
    {
        public PlayerModel PlayerMod { get; private set; }
        public GameModel GameMod { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            PlayerMod = Resources.Load<PlayerModel>("Tsuki/PlayerModel");
            GameMod = Resources.Load<GameModel>("Tsuki/GameModel");
            PlayerMod.Init();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += (_, _) =>
            {
                PlayerMod.Init();
            };
        }
    }
}
