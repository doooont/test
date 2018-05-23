using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

using Fishtail.PlayTheBall.Player;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 让其上方物体停下并等待用户下一次弹射的停止器。
    /// </summary>
    public class Stopper : MonoBehaviour
    {
        public GearEvent onStop;
        public GearEvent onRelease;

        /// <summary>
        /// 停止器当前所承载的对象。
        /// </summary>
        public GameObject containedObject { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (containedObject != null || other.gameObject.tag != "Player") {
                return;
            }
            this.StartCoroutine(this.DoStop(other.gameObject));
        }

        private IEnumerator DoStop(GameObject gobj)
        {
            containedObject = gobj;
            onStop.Invoke(containedObject);

            var rigid = gobj.GetComponent<Rigidbody>();
            rigid.isKinematic = true;

            // 将物体移动至中心。
            var targetPos = transform.position + new Vector3(0f, rigid.position.y, 0f);
            var dampVelocity = Vector3.zero;

            while (Vector3.Distance(rigid.position, targetPos) > 0.05f) {
                rigid.position = Vector3.SmoothDamp(
                    rigid.position, targetPos, ref dampVelocity, 0.07f);
                yield return null;
            }

            // 如果物体有发射器组件，则等到物体再一次被发射后再将调用释放方法
            // 否则直接释放球体。
            var launcher = gobj.GetComponentInChildren<Launcher>();
            if (launcher != null) {
                UnityAction action = null;
                action = () => {
                    this.ReleaseContainedObject();
                    // 确保事件只监听一次。
                    launcher.onLaunch.RemoveListener(action);
                };
                launcher.onLaunch.AddListener(action);
            }
            else {
                this.ReleaseContainedObject();
            }
        }

        private void ReleaseContainedObject()
        {
            var rigid = containedObject.GetComponent<Rigidbody>();
            if (rigid != null) {
                rigid.isKinematic = false;
            }
            onRelease.Invoke(containedObject);
            containedObject = null;
        }
    }
}