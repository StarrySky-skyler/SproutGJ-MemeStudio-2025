// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/09 01:02
// @version: 1.0
// @description:
// *****************************************************************************

using UnityEngine;

namespace Tsuki.Effects
{
    public class CursorTrail : MonoBehaviour
    {
        public float distance = 10f;
        public Vector3 offset = Vector3.zero;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!Camera.main)
            {
                Debug.LogWarning("主相机不存在，渲染拖尾失败");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 newPos = ray.GetPoint(distance) + offset;
            newPos.z = 0f;
            transform.position = newPos;
        }
    }
}
