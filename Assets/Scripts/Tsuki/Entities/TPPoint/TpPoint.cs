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
    public class TpPoint : MonoBehaviour
    {
        public Transform targetP;

        public void Tp(Transform self)
        {
            self.position = targetP.position;
            self.position = new Vector3(self.position.x, self.position.y, 0);
        }
    }
}
