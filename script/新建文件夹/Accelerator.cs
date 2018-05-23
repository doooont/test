using System;
using System.Collections;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 对其上方物体施加冲击力的加速器装置。
    /// </summary>
    public class Accelerator : MonoBehaviour
    {
        [Tooltip("加速物体时使用的冲击力大小。")]
        public float impulse = 10f;

        public GearEvent onAccelerate;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player") {
                return;
            }

            var rigid = other.GetComponent<Rigidbody>();
            if (rigid == null) { return; }

            rigid.AddForce(transform.forward * impulse, ForceMode.Impulse);
            onAccelerate.Invoke(rigid.gameObject);
        }
    }
}
