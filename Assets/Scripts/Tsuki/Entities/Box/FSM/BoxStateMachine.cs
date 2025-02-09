// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 13:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using System.Collections.Generic;
using Tsuki.Base;
using Tsuki.Entities.Box.FSM.Interfaces;
using Tsuki.Entities.Box.FSM.Types;
using Tsuki.Entities.Box.Types;
using Tsuki.Entities.IceLine;
using UnityEngine;

namespace Tsuki.Entities.Box.FSM
{
    /// <summary>
    ///     状态机上下文
    /// </summary>
    public class Context
    {
        public BoxType BoxType;
        public Func<Vector2Int, bool> CheckTp;
        public IceType IceType;
        public Vector2Int PushDirection;
        public Action<Transform> Tp;
    }

    public class BoxStateMachine
    {
        // 状态字典
        private readonly Dictionary<BoxStateType, IBoxState>
            _statesDict = new();

        // 当前状态
        private IBoxState _currentState;

        /// <summary>
        ///     添加状态
        /// </summary>
        /// <param name="boxStateType"></param>
        /// <param name="state"></param>
        public void AddState(BoxStateType boxStateType, IBoxState state)
        {
            if (!_statesDict.TryAdd(boxStateType, state))
            {
                DebugYumihoshi.Error<BoxStateMachine>("箱子状态机",
                    $"添加状态失败，{boxStateType}已存在");
                return;
            }

            DebugYumihoshi.Log<BoxStateMachine>("箱子状态机",
                $"添加状态成功{boxStateType}");
        }

        /// <summary>
        ///     移除状态
        /// </summary>
        /// <param name="boxStateType"></param>
        public void RemoveState(BoxStateType boxStateType)
        {
            if (!_statesDict.Remove(boxStateType))
            {
                DebugYumihoshi.Error<BoxStateMachine>("箱子状态机",
                    $"移除状态失败，{boxStateType}不存在");
                return;
            }

            DebugYumihoshi.Log<BoxStateMachine>("箱子状态机",
                $"移除状态成功{boxStateType}");
        }

        /// <summary>
        ///     转换状态
        /// </summary>
        /// <param name="boxStateType"></param>
        /// <param name="contextNextCheck"></param>
        /// <param name="contextNextEnter"></param>
        /// <param name="contextLastExit"></param>
        /// <returns></returns>
        public bool SwitchState(BoxStateType boxStateType,
            Context contextNextCheck = null,
            Context contextNextEnter = null, Context contextLastExit = null)
        {
            if (!_statesDict.TryGetValue(boxStateType,
                    out IBoxState targetIBoxState))
            {
                DebugYumihoshi.Error<BoxStateMachine>("箱子状态机",
                    $"转换状态失败，{boxStateType}不存在");
                return false;
            }

            if (!targetIBoxState.OnCheck(contextNextCheck))
            {
                DebugYumihoshi.Warn<BoxStateMachine>("箱子状态机",
                    $"转换状态错误{boxStateType}在check时失败");
                return false;
            }

            DebugYumihoshi.Log<BoxStateMachine>("箱子状态机",
                $"转换状态成功>>>当前状态{boxStateType}");
            _currentState?.OnExit(contextLastExit);
            _currentState = _statesDict[boxStateType];
            _currentState.OnEnter(contextNextEnter);
            return true;
        }

        public void OnUpdate()
        {
            _currentState?.OnUpdate();
        }
    }
}
