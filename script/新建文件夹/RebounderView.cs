using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 反弹器的视图控制器。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class RebounderView : MonoBehaviour
    {
        private Animator _animator;
        private Rebounder _rebounder;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            _rebounder = this.GetComponentInParent<Rebounder>();
            _rebounder.onRebound.AddListener(this.OnRebound);
        }

        private void OnRebound(GameObject gobj) =>
            _animator.SetTrigger("Rebound");
    }
}
