// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/27 20:01
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using System.Collections.Generic;
using DG.Tweening;
using Tsuki.Base;
using Tsuki.Entities.Box.FSM;
using Tsuki.Entities.Box.FSM.BoxStates;
using Tsuki.Entities.IceLine;
using Tsuki.Entities.TPPoint;
using Tsuki.Interface;
using Tsuki.Managers;
using Tsuki.MVC.Models.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tsuki.Entities.Box
{
    public class BoxEntity : MonoBehaviour, IUndoable
    {
        [Header("箱子类型")] public BoxType boxType;

        [Header("冰块层")] public LayerMask groundIceLayer;
        public LayerMask groundIceLineLayer;

        [Header("TP层")] public LayerMask tpLayer;

        private bool _added;
        [HideInInspector] public Vector2Int lastPushDirection;
        public Tween MoveTween { get; set; }

        public BoxStateMachine StateMachine;

        private Vector3 _newPos;
        private Vector3 _startPos;
        private Stack<Vector3> _lastPosStack;

        private void Awake()
        {
            _lastPosStack = new Stack<Vector3>();
            // 初始化状态机
            StateMachine = new BoxStateMachine();
            StateMachine.AddState(BoxStateType.Idle, new BoxIdleState(this));
            StateMachine.AddState(BoxStateType.IceSlide,
                new BoxIceSlideState(this));
            StateMachine.AddState(BoxStateType.Tp, new BoxTpState(this));
            StateMachine.AddState(BoxStateType.PushMoving,
                new BoxPushMovingState(this));
        }

        private void Start()
        {
            _startPos = transform.position;
            _newPos = transform.position;
            // 初始化状态机
            StateMachine.SwitchState(BoxStateType.Idle);
        }

        private void OnEnable()
        {
            GameManager.Instance.onGameUndo.AddListener(Undo);
        }

        private void OnDisable()
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
