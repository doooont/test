using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 随机产生 3、4、5 个球体的老虎机。默认情况下只能使用一次。
    /// </summary>
    public class SlotMachine : MonoBehaviour
    {
        /// <summary>
        /// 幸运值。
        /// </summary>
        public enum Luck
        {
            Normal,
            Rare,
            SuperRare
        }

        [Serializable]
        public class DrawnEvent : UnityEvent<Luck> { }

        [Tooltip("老虎机计算幸运值需要等待的时间。")]
        public float drawTime = 3f;
        [Tooltip("老虎机根据幸运值产生物体前需要等到的时间。")]
        public float spawnTime = 1f;
        [Tooltip("对新产生的物体施加的冲击力大小。")]
        public float impulse = 10f;
        [Tooltip("物体的随机生成点。")]
        public Transform spawnPivots;

        public UnityEvent onActivated;
        public DrawnEvent onDraw;
        public UnityEvent onDeactivated;

        /// <summary>
        /// 老虎机是否已被使用过。
        /// </summary>
        public bool used { get; private set; }

        private HashSet<int> _usedPivots = new HashSet<int>();

        private void OnCollisionEnter(Collision collision)
        {
            if (used) { return; }

            this.StartCoroutine(this.DoSpawn(collision.rigidbody));
        }

        /// <summary>
        /// 重设老虎机。
        /// </summary>
        public void Reset()
        {
            used = false;
            _usedPivots.Clear();
        }

        private IEnumerator DoSpawn(Rigidbody rigid)
        {
            used = true;

            rigid.isKinematic = true;
            rigid.position = transform.position;

            onActivated.Invoke();

            yield return new WaitForSeconds(drawTime);

            var luck = (Luck)UnityEngine.Random.Range(0, 3);
            onDraw.Invoke(luck);

            yield return new WaitForSeconds(spawnTime);

            rigid.isKinematic = false;
            this.Spawn(rigid.gameObject);

            int spawnCount = 0;
            switch (luck) {
            case Luck.Normal:
                spawnCount = 2;
                break;
            case Luck.Rare:
                spawnCount = 3;
                break;
            case Luck.SuperRare:
                spawnCount = 4;
                break;
            }

            for (int i = 0; i < spawnCount; ++i) {
                this.Spawn();
            }

            onDeactivated.Invoke();
        }

        private void Spawn() =>
            this.Spawn(GameManager.instance.CreateSphere());

        private void Spawn(GameObject gobj)
        {
            // 每一个生成点只能使用一次。
            int pivotIndex = UnityEngine.Random.Range(0, spawnPivots.childCount);
            while (_usedPivots.Contains(pivotIndex)) {
                pivotIndex = UnityEngine.Random.Range(0, spawnPivots.childCount);
            }

            var pivot = spawnPivots.GetChild(pivotIndex);
            gobj.transform.SetPositionAndRotation(pivot.position, pivot.rotation);
            gobj.GetComponent<Rigidbody>().AddForce(pivot.forward * impulse, ForceMode.Impulse);

            _usedPivots.Add(pivotIndex);
        }
    }
}
