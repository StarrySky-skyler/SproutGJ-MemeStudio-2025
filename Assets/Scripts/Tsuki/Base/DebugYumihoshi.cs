// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/09 13:02
// @version: 1.0
// @description:
// *****************************************************************************

using UnityEngine;

namespace Tsuki.Base
{
    public static class DebugYumihoshi
    {
        /// <summary>
        /// 打印日志
        /// </summary>
        /// <param name="funcType">功能模块类型名</param>
        /// <param name="msg">日志消息</param>
        /// <param name="context">其他</param>
        /// <typeparam name="T">当前脚本类型</typeparam>
        public static void Log<T>(string funcType, object msg,
            Object context = null)
        {
            Debug.Log($"[{funcType}] {typeof(T).Name}\n>>> {msg}", context);
        }

        /// <summary>
        /// 打印警告
        /// </summary>
        /// <param name="funcType">功能模块类型名</param>
        /// <param name="msg">日志消息</param>
        /// <param name="context">其他</param>
        /// <typeparam name="T">当前脚本类型</typeparam>
        public static void Warn<T>(string funcType, object msg,
            Object context = null)
        {
            Debug.LogWarning($"[{funcType}] {typeof(T).Name}\n>>> {msg}",
                context);
        }

        /// <summary>
        /// 打印错误
        /// </summary>
        /// <param name="funcType">功能模块类型名</param>
        /// <param name="msg">日志消息</param>
        /// <param name="context">其他</param>
        /// <typeparam name="T">当前脚本类型</typeparam>
        public static void Error<T>(string funcType, object msg,
            Object context = null)
        {
            Debug.LogError($"[{funcType}] {typeof(T).Name}\n>>> {msg}",
                context);
        }
    }
}
