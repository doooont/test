using System;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 目的地。
    /// </summary>
    public class Destination : MonoBehaviour
    {
        [Tooltip("目的地达成成功条件所需要进入的球体数量。")]
        public int requiredCount = 1;

        public GearEvent onArrived;
        public UnityEvent onSucceed;

        /// <summary>
        /// 目前已经抵达的球体的数目。
        /// </summary>
        public int arrivedCount { get; private set; }

        /// <summary>
        /// 该目的地是否已经满足通关条件。
        /// </summary>
        public bool succeed { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            var gobj = collision.gameObject;
            var rigid = collision.rigidbody;

            rigid.isKinematic = true;
            rigid.position = transform.position;

            onArrived.Invoke(gobj);
            ++arrivedCount;

            if (arrivedCount > requiredCount && !succeed) {
                succeed = true;
                onSucceed.Invoke();
            }
        }
    }
}
