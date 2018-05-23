using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 为弹射器提供通用的视图控制逻辑。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public abstract class BaseEmitterView : MonoBehaviour
    {
        protected Animator animator { get; private set; }
        protected BaseEmitter emitter { get; private set; }

        protected virtual void Awake()
        {
            animator = this.GetComponent<Animator>();
            emitter = this.GetComponentInParent<BaseEmitter>();
            emitter.onActivated.AddListener(this.OnActivate);
            emitter.onEmit.AddListener(this.OnEmit);
        }

        protected virtual void OnActivate(GameObject target)
        {
            // 根据发射器设置的时间来调整动画播放的速度，下同。
            animator.SetFloat("ActivateSpeed", 1f / emitter.emitDelay);
            animator.SetTrigger("Activate");
        }

        protected void OnEmit(GameObject target)
        {
            animator.SetFloat("EmitSpeed", 1f / emitter.coolingTime);
            animator.SetTrigger("Emit");
        }
    }
}
