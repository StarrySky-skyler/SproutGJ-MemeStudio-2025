// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM.Base;
using Tsuki.Entities.Box.FSM.Interfaces;
using Tsuki.Entities.Box.FSM.Types;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxPushMovingState : BoxState, IBoxState
    {
        private readonly RaycastHit2D[] _hitsBuffer = new RaycastHit2D[10];

        public BoxPushMovingState(BaseObj baseObj) : base(baseObj)
        {
        }


        public void OnEnter(Context context)
        {
            Move();
        }


        public void OnUpdate(Context context)
        {
        }


        public void OnExit(Context context)
        {
        }

        public bool OnCheck(Context context)
        {
            // 检测是否可以推动
            return GetPushable(context.PushDirection);
        }

        private void Move()
        {
            // 移动
            BaseObj.MoveTween = BaseObj.transform.DOMove(BaseObj.NewPos,
                ModelsManager.Instance.PlayerMod.moveTime);
            BaseObj.MoveTween.onComplete += () =>
            {
                BaseObj.StateMachine.SwitchState(BoxStateType.Idle);
            };
        }

        /// <summary>
        ///     获取箱子是否可推动
        /// </summary>
        /// <returns></returns>
        private bool GetPushable(Vector2Int pushDirection)
        {
            SetNewPos(pushDirection);
            Debug.DrawRay(BaseObj.transform.position,
                (Vector2)pushDirection,
                Color.green, 3);
            // 射线检测是否还有箱子或墙
            int hitCount = Physics2D.RaycastNonAlloc(
                BaseObj.transform.position,
                pushDirection, _hitsBuffer,
                Vector2.Distance(BaseObj.transform.position,
                    BaseObj.NewPos),
                ModelsManager.Instance.GameMod.obstacleLayer);

            for (int i = 0; i < hitCount; i++)
                if (_hitsBuffer[i].collider !=
                    BaseObj.GetComponent<Collider2D>())
                    return false;

            // 检测草
            hitCount = Physics2D.RaycastNonAlloc(
                BaseObj.transform.position,
                pushDirection, _hitsBuffer,
                Vector2.Distance(BaseObj.transform.position,
                    BaseObj.NewPos),
                ModelsManager.Instance.GameMod.grassLayer);

            for (int i = 0; i < hitCount; i++)
                if (_hitsBuffer[i].collider !=
                    BaseObj.GetComponent<Collider2D>() &&
                    !BaseObj.CompareTag("Weeders"))
                    return false;

            return Commons.IsOnMap(ModelsManager.Instance.GameMod,
                BaseObj.NewPos);
        }

        /// <summary>
        ///     设置新位置
        /// </summary>
        private void SetNewPos(Vector2Int pushDirection)
        {
            BaseObj.lastPushDirection = pushDirection;
            BaseObj.NewPos = BaseObj.transform.position +
                             new Vector3(
                                 pushDirection.x *
                                 ModelsManager.Instance.GameMod.girdSize,
                                 pushDirection.y *
                                 ModelsManager.Instance.GameMod.girdSize,
                                 0);
        }
    }
}
