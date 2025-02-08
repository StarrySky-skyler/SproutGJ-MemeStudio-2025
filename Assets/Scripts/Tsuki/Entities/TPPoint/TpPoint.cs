// *****************************************************************************
// @author: 绘星tsuki
// @email: xiaoyuesun915@gmail.com
// @creationDate: 2025/02/07 17:02
// @version: 1.0
// @description:
// *****************************************************************************

using System.Collections;
using Tsuki.Entities.Box.Base;
using Tsuki.Entities.Box.FSM;
using Tsuki.Entities.Box.FSM.Types;
using UnityEngine;

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

        public Transform LastTped { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Box")) return;
            BaseObj box = other.gameObject.GetComponent<BaseObj>();
            switch (gameObject.name)
            {
                case "P1":
                    if (transform.parent.Find("P2").GetComponent<TpPoint>()
                            .LastTped == other.gameObject.transform)
                        return;
                    break;
                case "P2":
                    if (transform.parent.Find("P1").GetComponent<TpPoint>()
                            .LastTped == other.gameObject.transform)
                        return;
                    break;
                default:
                    Debug.LogError("TP点名称错误");
                    return;
            }

            box.StateMachine.SwitchState(BoxStateType.Tp,
                new Context { CheckTp = CheckTp },
                new Context { Tp = Tp });
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            LastTped = null;
        }

        private bool CheckTp(Vector2Int enterDirection)
        {
            switch (tpType)
            {
                case TpType.None:
                    Debug.LogError("TP点类型为空");
                    return false;
                case TpType.Vertical:
                    if (enterDirection.x != 0) return false;
                    break;
                case TpType.Horizontal:
                    if (enterDirection.y != 0) return false;
                    break;
                default:
                    Debug.LogError("TP点类型错误");
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     处理TP点
        /// </summary>
        /// <param name="self">要传送物体的transform</param>
        private void Tp(Transform self)
        {
            LastTped = self;
            StartCoroutine(TpCoroutine(self));
        }

        private IEnumerator TpCoroutine(Transform self)
        {
            Vector3 targetPos = new(targetP.position.x,
                targetP.position.y, 0);
            self.position = targetPos;
            yield return new WaitForSeconds(0.1f);
            while (self.position != targetPos)
            {
                self.position = new Vector3(targetP.position.x,
                    targetP.position.y, 0);
                yield return null;
            }
        }
    }
}
