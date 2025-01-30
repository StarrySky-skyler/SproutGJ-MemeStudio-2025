// ********************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/01/28 22:01
// @version: 1.0
// @description:
// ********************************************************************************

using UnityEngine;
using UnityEngine.Pool;

namespace Tsuki.Effects
{
    public class Footprint : MonoBehaviour
    {
        public ObjectPool<GameObject> footPool;

        public void DestorySelf()
        {
            if(gameObject.activeSelf)
            footPool.Release(gameObject);
        }

        //public void DestorySelf()
        //{
        //    Destroy(gameObject);
        //}
    }
}
