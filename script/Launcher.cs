using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.Player
{
    /// <summary>
    /// LauncherController 实现控制球体进行发射的装置。
    /// </summary>
    public class Launcher
        : MonoBehaviour
    {
        [Tooltip("发射的对象。")]
        public Rigidbody targetRigidbody;
        [Tooltip("发射弹球时使用的额外冲击力大小。")]
        public float additionalImpulse = 1f;
        [Tooltip("装置能产生的最大冲击。")]
        public float maximumImpulse = 10f;
        [Tooltip("确认发射后施加冲击力的延迟。")]
        public float launchDelay = 0.5f;

        public UnityEvent onAwake;
        public UnityEvent onActivate;
        public UnityEvent onDeactivate;
        public UnityEvent onLaunch;
        public UnityEvent onStop;

        /// <summary>
        /// 装置是否被唤醒（玩家开始按住碰撞体）。
        /// </summary>
        public bool awaked { get; private set; }

        /// <summary>
        /// 装置是否被激活（玩家将手指拖拽至碰撞体外）。
        /// </summary>
        public bool activated { get; private set; }

        /// <summary>
        /// 装置在发射球体时的方向。
        /// </summary>
        public Vector3 impulseDirection { get; private set; } = Vector3.up;

        /// <summary>
        /// 装置在发射球体时采用的冲击力。
        /// </summary>
        public float currentImpulse { get; private set; }

        /// <summary>
        /// 设置装置发射球体时冲击力的大小与方向。注意实际冲击力的大小会受到额外冲击力和最大冲击力值的影响。
        /// </summary>
        /// <param name="impulse">代表冲击力的向量。</param>
        public void SetImpulse(Vector3 impulse)
        {
            impulseDirection = Vector3.ProjectOnPlane(impulse.normalized, Vector3.up);
            currentImpulse = Mathf.Clamp(additionalImpulse + impulse.magnitude, 0f, maximumImpulse);
        }

        /// <summary>
        /// 唤醒装置。
        /// </summary>
        public void AwakeLauncher()
        {
            if (awaked) { return; }

            awaked = true;
            onAwake.Invoke();
        }

        /// <summary>
        /// 激活装置。
        /// </summary>
        public void Activate()
        {
            if (!awaked) {
                Debug.LogError("Launcher has not been awaked.");
                return;
            }
            if (activated) { return; }

            activated = true;
            onActivate.Invoke();
        }

        /// <summary>
        /// 反激活装置。
        /// </summary>
        public void Deactivate()
        {
            if (!awaked) {
                Debug.LogError("Launcher has not been awaked.");
                return;
            }
            if (!activated) { return; }

            activated = false;
            onDeactivate.Invoke();
        }

        /// <summary>
        /// 立即终止发射进程。
        /// </summary>
        public void StopLaunch()
        {
            if (!awaked) { return; }

            this.ClearStates();
            onStop.Invoke();
        }

        /// <summary>
        /// 发射球体。
        /// </summary>
        public void Launch()
        {
            if (!awaked || !activated) {
                Debug.LogError("Launcher has not been awaked or activated.");
                return;
            }

            this.StartCoroutine(this.DoLaunch());
            onLaunch.Invoke();
        }

        private IEnumerator DoLaunch()
        {
            yield return new WaitForSeconds(launchDelay);

            targetRigidbody.isKinematic = false;
            targetRigidbody.AddForce(impulseDirection * currentImpulse, ForceMode.Impulse);

            this.ClearStates();

        }

        /// <summary>
        /// 清空装置状态。
        /// </summary>
        private void ClearStates()
        {
            awaked = false;
            activated = false;
            impulseDirection = Vector3.up;
            currentImpulse = 0f;
        }
    }
}
