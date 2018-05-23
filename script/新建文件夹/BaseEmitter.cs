using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 为涉及到弹射逻辑的装置提供统一的功能。
    /// </summary>
    public abstract class BaseEmitter : MonoBehaviour
    {
        [Tooltip("物体实际被发射的延迟。")]
        public float emitDelay = 0.5f;
        [Tooltip("进行下一次发射之前所需的冷却时间。")]
        public float coolingTime = 0.3f;
        [Tooltip("发射物体时使用的冲击力。")]
        public float impulse = 40f;

        public GearEvent onActivated;
        public GearEvent onEmit;

        /// <summary>
        /// 准备施加弹射力的目标对象。不为空则说明弹射器处于激活状态。
        /// </summary>
        public GameObject target { get; private set; }

        /// <summary>
        /// 发射器是否正在冷却中。
        /// </summary>
        public bool cooling { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            if (target != null) {
                // 弹射器一次只能弹射一个物体。
                return;
            }

            var collidedObj = collision.gameObject;
            this.StartCoroutine(this.DoEmit(collidedObj));
        }

        /// <summary>
        /// 获取发射点。
        /// </summary>
        protected abstract Transform GetEmitPivot();

        private IEnumerator DoEmit(GameObject target)
        {
            this.target = target;

            // 在没有发射之前，使目标对象刚体静止并移动至中心位置。
            var targetRigid = target.GetComponent<Rigidbody>();
            targetRigid.isKinematic = true;
            target.transform.position = transform.position;

            while (cooling) {
                yield return null;
            }

            onActivated.Invoke(target);

            yield return new WaitForSeconds(emitDelay);

            var pivot = this.GetEmitPivot();
            target.transform.position = pivot.position;
            targetRigid.isKinematic = false;
            targetRigid.AddForce(pivot.forward * impulse, ForceMode.Impulse);

            onEmit.Invoke(gameObject);

            // 在弹射完毕后等待一帧再清空 target，这是为了避免物体在弹射完毕后再一次被当前弹射器捕捉。
            yield return null;
            this.target = null;

            // 在 target 被设置成 null 以后，发射器已经可以接受下一个弹射目标，但新创建的 DoEmit
            // 协程会等待冷却结束后才实施弹射任务。
            cooling = true;
            yield return new WaitForSeconds(coolingTime);
            cooling = false;
        }
    }
}
