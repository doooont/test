using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 加速器的视图控制组建。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AcceleratorView : MonoBehaviour
    {
        private Animator _animator;
        private Accelerator _accelerator;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            _accelerator = this.GetComponentInParent<Accelerator>();
            _accelerator.onAccelerate.AddListener(this.OnAccelerate);
        }

        private void OnAccelerate(GameObject gobj) =>
            _animator.SetTrigger("Accelerate");
    }
}
