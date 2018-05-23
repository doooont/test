using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 能够让球体实现空间传送的传送门。
    /// </summary>
    public class Portal : MonoBehaviour
    {
        [Tooltip("球体的出口。")]
        public Portal exit;
        [Tooltip("球的掉落与浮出速度。")]
        public float fallSpeed = 5f;

        public GearEvent onEnter;
        public GearEvent onExit;

        private static Lazy<int> s_ignorePortalLayer =
            new Lazy<int>(() => LayerMask.NameToLayer("Ignore Portal"));

        private void Awake()
        {
            transform.Find("ExitingObjectTrigger").gameObject
                .AddComponent<ExitingObjectListener>().portal = this;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var rigid = collision.rigidbody;
            this.StartCoroutine(this.DoTeleport(rigid, collision.relativeVelocity));
        }

        private IEnumerator DoTeleport(Rigidbody rigid, Vector3 velocity)
        {
            // 刚体在传送前的高度会被保存下来，并在传送结束后恢复。
            var height = rigid.position.y;
            var dampVelocity = Vector3.zero;

            rigid.isKinematic = true;

            // 将物体的位置平滑地移动至传送门中央。
            while (Vector3.Distance(rigid.position, transform.position) > 0.05f) {
                rigid.position = Vector3.SmoothDamp(
                    rigid.position, transform.position, ref dampVelocity, 0.07f);
                yield return null;
            }

            // 使物体下沉。
            while (transform.position.y - rigid.position.y < height) {
                var pos = rigid.position;
                pos.y -= fallSpeed * Time.deltaTime;
                rigid.position = pos;
                yield return null;
            }

            onEnter.Invoke(rigid.gameObject);
            exit?.StartCoroutine(exit.DoExpel(rigid, height, velocity));
        }

        private IEnumerator DoExpel(Rigidbody rigid, float height, Vector3 velocity)
        {
            var dampVelocity = Vector3.zero;

            rigid.position = transform.position + new Vector3(0f, -height, 0f);

            // 使物体升起。
            while (rigid.position.y - transform.position.y < height) {
                var pos = rigid.position;
                pos.y += fallSpeed * Time.deltaTime;
                rigid.position = pos;
                yield return null;
            }

            // 恢复刚体状态。
            rigid.isKinematic = false;
            rigid.velocity = velocity;

            // 在物体离开传送们之前，将物体的层级设置为 “Ignore Portal”。
            // 该层级的刚体无法与传送门碰撞，这避免了刚被传送出去的物体再一次被传送回原来的传送门。
            var initialLayer = rigid.gameObject.layer;
            rigid.gameObject.layer = s_ignorePortalLayer.Value;

            var comp = rigid.gameObject.AddComponent<ExitingObject>();
            comp.portal = this;
            comp.initialLayer = initialLayer;

            onExit.Invoke(rigid.gameObject);
        }

        private class ExitingObjectListener : MonoBehaviour
        {
            public Portal portal;

            private void OnTriggerExit(Collider collider)
            {
                var exitingObj = collider.gameObject.GetComponent<ExitingObject>();
                if (exitingObj == null || exitingObj.portal != portal) {
                    return;
                }
                collider.gameObject.layer = exitingObj.initialLayer;
                Destroy(exitingObj);
            }
        }

        private class ExitingObject : MonoBehaviour
        {
            public Portal portal;
            public int initialLayer;
        }
    }
}
