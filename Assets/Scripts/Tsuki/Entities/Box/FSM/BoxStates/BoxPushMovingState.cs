// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 14:02
// @version: 1.0
// @description:
// *****************************************************************************

using DG.Tweening;
using Tsuki.Base;
using Tsuki.Entities.Box.FSM.Interface;
using Tsuki.Entities.IceLine;
using Tsuki.Entities.TPPoint;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM.BoxStates
{
    public class BoxPushMovingState : BoxState, IBoxState
    {
        private readonly RaycastHit2D[] _hitsBuffer = new RaycastHit2D[10];

        public BoxPushMovingState(BoxEntity boxEntity) : base(boxEntity)
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

        /// <summary>
        /// 推动箱子
        /// </summary>
        /// <returns></returns>
        // private bool TryPushBox(Vector2Int pushDirection)
        // {
        //     if (!GetPushable(pushDirection)) return false;
        //     Move();
        //     return true;
        // }
        private void Move()
        {
            // 移动
            BoxEntity.MoveTween = BoxEntity.transform.DOMove(BoxEntity.NewPos,
                    ModelsManager.Instance.PlayerMod.moveTime)
                .OnComplete(
                    () =>
                    {
                        // 成功转换TP状态，直接返回
                        if (BoxEntity.StateMachine.SwitchState(BoxStateType.Tp))
                            return;
                        if (BoxEntity.StateMachine.SwitchState(BoxStateType.IceSlide))
                            return;
                        BoxEntity.StateMachine.SwitchState(BoxStateType.Idle);
                    });
        }

        /// <summary>
        /// 获取箱子是否可推动
        /// </summary>
        /// <returns></returns>
        private bool GetPushable(Vector2Int pushDirection)
        {
            SetNewPos(pushDirection);
            Debug.DrawRay(BoxEntity.transform.position,
                (Vector2)pushDirection,
                Color.green, 3);
            // 射线检测是否还有箱子或墙
            int hitCount = Physics2D.RaycastNonAlloc(
                BoxEntity.transform.position,
                pushDirection, _hitsBuffer,
                Vector2.Distance(BoxEntity.transform.position,
                    BoxEntity.NewPos),
                ModelsManager.Instance.PlayerMod.obstacleLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitsBuffer[i].collider !=
                    BoxEntity.GetComponent<Collider2D>())
                    return false;
            }

            return Commons.IsOnMap(ModelsManager.Instance.PlayerMod,
                BoxEntity.NewPos);
        }

        /// <summary>
        /// 设置新位置
        /// </summary>
        private void SetNewPos(Vector2Int pushDirection)
        {
            BoxEntity.lastPushDirection = pushDirection;
            BoxEntity.NewPos = BoxEntity.transform.position +
                               new Vector3(
                                   pushDirection.x *
                                   ModelsManager.Instance.PlayerMod.girdSize,
                                   pushDirection.y *
                                   ModelsManager.Instance.PlayerMod.girdSize,
                                   0);
        }

        /// <summary>
        /// 处理TP
        /// </summary>
        private bool HandleTp()
        {
            Collider2D hit1 =
                Physics2D.OverlapPoint(BoxEntity.NewPos, BoxEntity.tpLayer);
            if (!hit1) return false;
            TpPoint tpPoint = hit1.GetComponent<TpPoint>();
            tpPoint.Tp(BoxEntity.transform, BoxEntity.lastPushDirection);
            return true;
        }
    }
}
