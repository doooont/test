using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 停止器的视图控制组建。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class StopperView : MonoBehaviour
    {
        private Animator _animator;
        private Stopper _stopper;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            _stopper = this.GetComponentInParent<Stopper>();
            _stopper.onStop.AddListener(this.OnStop);
            _stopper.onRelease.AddListener(this.OnRelease);
        }

        private void OnStop(GameObject gobj) =>
            _animator.SetBool("Active", true);

        private void OnRelease(GameObject gobj) =>
            _animator.SetBool("Active", false);
    }
}
