using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 复制器的视图控制组件。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class DuplicatorView : MonoBehaviour
    {
        private Animator _animator;
        private Duplicator _duplicator;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            _duplicator = this.GetComponentInParent<Duplicator>();
            _duplicator.onDuplicate.AddListener(this.OnDuplicate);
        }

        private void OnDuplicate(GameObject gobj, GameObject newGObj) =>
            _animator.SetTrigger("Duplicate");
    }
}
