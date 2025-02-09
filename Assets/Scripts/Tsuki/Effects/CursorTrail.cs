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
        public new Camera camera;
        public float distance = 10f;
        public Vector3 offset = Vector3.zero;

        private void Awake()
        {
            DontDestroyOnLoad(transform.parent.parent);
        }

        private void Update()
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Vector3 newPos = ray.GetPoint(distance) + offset;
            newPos.z = 0f;
            transform.position = newPos;
        }
    }
}
