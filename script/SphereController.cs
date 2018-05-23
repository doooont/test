using System;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.Player
{
    /// <summary>
    /// 控制球体运作逻辑的组件。
    /// </summary>
    public class SphereController : MonoBehaviour
    {
        /// <summary>
        /// 球体在静止时是否会自动被销毁。
        /// </summary>
        public bool destroyOnSleep { get; set; } = true;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = this.GetComponent<Rigidbody>();
            _rigidbody.sleepThreshold = 0.02f;
        }

        private void FixedUpdate()
        {
            if (destroyOnSleep && !_rigidbody.isKinematic && _rigidbody.IsSleeping()) {
                Destroy(gameObject);
            }
        }
    }
}
