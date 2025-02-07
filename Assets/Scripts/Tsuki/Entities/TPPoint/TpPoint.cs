// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/06 21:02
// @version: 1.0
// @description:
// *****************************************************************************

using System;
using DG.Tweening;
using Tsuki.Entities.Box;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tsuki.Entities.TPPoint
{
    public enum TpType
    {
        None,
        Vertical,
        Horizontal
    }
    
    public class TpPoint : MonoBehaviour
    {
        public Transform targetP;
        public TpType tpType;

        /// <summary>
        /// 处理TP点
        /// </summary>
        /// <param name="self">要传送物体的transform</param>
        /// <param name="enterDirection">进入时的移动方向</param>
        public void Tp(Transform self, Vector2Int enterDirection)
        {
            switch (tpType)
            {
                case TpType.None:
                    Debug.LogError("TP点类型为空");
                    break;
                case TpType.Vertical:
                    if (enterDirection.x!= 0) return;
                    break;
                case TpType.Horizontal:
                    if (enterDirection.y!= 0) return;
                    break;
                default:
                    Debug.LogError("TP点类型错误");
                    break;
            }
            self.position = targetP.position;
            self.position = new Vector3(self.position.x, self.position.y, 0);
        }
    }
}
