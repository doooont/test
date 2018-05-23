using System;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 能够在反弹时改变球体动能的反弹器。
    /// </summary>
    public class Rebounder : MonoBehaviour
    {
        [Tooltip("撞击反弹器的球体的速度将乘上该因数，1 代表不损失任何速度。")]
        public float velocityMultiplier = 1;

        public GearEvent onRebound;

        private void OnCollisionEnter(Collision collision)
        {
            collision.rigidbody.velocity *= velocityMultiplier;
            onRebound.Invoke(collision.gameObject);
        }
    }
}
