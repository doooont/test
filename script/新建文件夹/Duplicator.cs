using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 复制器能够在反弹物体的同时创造该物体的副本。新创建的物体将在撞击点的另一面出现，
    /// 并且保有原始物体撞击时的速度。
    /// </summary>
    public class Duplicator : MonoBehaviour
    {
        [Serializable]
        public class DuplicateEvent : UnityEvent<GameObject, GameObject> { }

        public DuplicateEvent onDuplicate;

        private Collider _collider;

        private HashSet<GameObject> _duplicates = new HashSet<GameObject>();

        private void Awake()
        {
            _collider = this.GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_duplicates.Remove(collision.gameObject)) {
                return;
            }

            var rigid = collision.rigidbody;

            var dir = collision.relativeVelocity.normalized;
            var ray = new Ray(collision.contacts[0].point + dir * 3f, -dir);

            RaycastHit hit;
            if (!_collider.Raycast(ray, out hit, 10f)) {
                Debug.LogError("This should not happen.");
                return;
            }

            var duplicate = GameManager.instance.CreateSphere();
            duplicate.transform.position = hit.point;
            duplicate.GetComponent<Rigidbody>().velocity = collision.relativeVelocity;

            onDuplicate.Invoke(gameObject, duplicate);
            _duplicates.Add(duplicate);
        }
    }
}
