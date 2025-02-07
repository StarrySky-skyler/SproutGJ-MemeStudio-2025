// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections.Generic;
using DG.Tweening;
using Tsuki.Entities.Box.FSM;
using Tsuki.Entities.Box.FSM.BoxStates;
using Tsuki.Entities.Box.Types;
using Tsuki.Interface;
using Tsuki.Managers;
using UnityEngine;

namespace Tsuki.Entities.Box.Base
{
    public class BaseObj : MonoBehaviour, IUndoable
    {
        [Header("箱子类型")] [SerializeField] public BoxType boxType;

        private bool _added;
        [HideInInspector] public Vector2Int lastPushDirection;
        public Tween MoveTween { get; set; }
        public Vector3 NewPos { get; set; }

        public BoxStateMachine StateMachine;

        private Vector3 _startPos;
        private Stack<Vector3> _lastPosStack;

        protected virtual void Awake()
        {
            _lastPosStack = new Stack<Vector3>();
            // 初始化状态机
            StateMachine = new BoxStateMachine();
            StateMachine.AddState(BoxStateType.Idle, new BoxIdleState(this));
            StateMachine.AddState(BoxStateType.PushMoving,
                new BoxPushMovingState(this));
        }

        protected virtual void Start()
        {
            _startPos = transform.position;
            NewPos = transform.position;
            // 初始化状态机
            StateMachine.SwitchState(BoxStateType.Idle);
        }

        protected virtual void OnEnable()
        {
            GameManager.Instance.onGameUndo.AddListener(Undo);
        }

        protected virtual void OnDisable()
        {
            if (!GameManager.Instance) return;
            GameManager.Instance.onGameUndo.RemoveListener(Undo);
        }

        public void Undo()
        {
            // 回到上一个位置
            if (!_lastPosStack.TryPop(out Vector3 result)) return;
            transform.position = result;
        }

        /// <summary>
        /// 重复上一个位置
        /// </summary>
        public void RepeatPos()
        {
            if (!_lastPosStack.TryPeek(out Vector3 result))
            {
                _lastPosStack.Push(_startPos);
                return;
            }

            _lastPosStack.Push(transform.position);
        }
    }
}
