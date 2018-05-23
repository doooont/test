using System;
using System.Collections.Generic;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 多米诺骨牌生成器在用户提供的路径点构成的路径上生成多米诺骨牌。
    /// </summary>
    public class DominoGenerator : MonoBehaviour
    {
        /// <summary>
        /// 生成多米诺骨牌时材质的切换模式。
        /// </summary>
        public enum MaterialAlternateMode
        {
            /// <summary>
            /// 每生成一个多米诺骨牌切换一次材质。
            /// </summary>
            EachDomino,

            /// <summary>
            /// 每完整生成一段路径切换一次材质。
            /// </summary>
            EachSegment
        }

        [Tooltip("多米诺骨牌的预制体。")]
        public GameObject dominoPrefab;
        [Tooltip("生成多米诺骨牌时使用的路径点集合。")]
        public Transform waypoints;
        [Tooltip("路径是否包围，即路径点集合的首尾是否相连。")]
        public bool encircle = false;
        [Tooltip("每个多米诺骨牌的间距。")]
        public float spacing = 0.3f;

        [Header("Materials")]
        [Tooltip("材质的切换模式。")]
        public MaterialAlternateMode alternateMode = MaterialAlternateMode.EachDomino;
        [Tooltip("可用材质集合。")]
        public List<Material> materials;

        /// <summary>
        /// 下一次生成多米诺骨牌时会使用的材质。
        /// </summary>
        public Material currentMaterial { get; private set; }

        /// <summary>
        /// 下一次生成多米诺骨牌时会使用的材质的索引。
        /// </summary>
        public int currentMaterialIndex { get; private set; }

        private void Awake()
        {
            currentMaterial = materials[0];

            for (int i = 1; i < waypoints.childCount; ++i) {
                var p1 = waypoints.GetChild(i - 1).position;
                var p2 = waypoints.GetChild(i).position;

                // 下一个线段的方向。
                var nextDir =
                    i != waypoints.childCount - 1
                    ? new Vector3?((waypoints.GetChild(i + 1).transform.position - p2).normalized) : null;

                this.GenerateFromSegment(p1, p2, i == 1 ? 0f : spacing, nextDir);
            }

            if (encircle) {
                var p1 = waypoints.GetChild(waypoints.childCount - 1).position;
                var p2 = waypoints.GetChild(0).position;
                this.GenerateFromSegment(p1, p2, spacing);
            }
        }

        private void GenerateFromSegment(Vector3 p1, Vector3 p2, float offset, Vector3? nextDirection = null) 
        {
            var dir = (p2 - p1).normalized;
            float dis = Vector3.Distance(p1, p2);
            float currDis = offset;

            while (currDis < dis) {
                var domino = Instantiate(dominoPrefab).transform;
                domino.Find("View").GetComponent<Renderer>().sharedMaterial = currentMaterial;
                domino.forward = dir;
                domino.position = p1 + dir * currDis;

                currDis += spacing;

                // 微调当前线段最后一个多米诺骨牌的方向与位置，使之倒后能够带动下一段多米诺骨牌。
                if (currDis >= dis && nextDirection != null) {
                    domino.forward += nextDirection.Value;
                    domino.position = (domino.position + p2) / 2f;
                }

                if (alternateMode == MaterialAlternateMode.EachDomino) {
                    this.SwitchMaterial();
                }
            }

            if (alternateMode == MaterialAlternateMode.EachSegment) {
                this.SwitchMaterial();
            }
        }

        private void SwitchMaterial()
        {
            ++currentMaterialIndex;

            if (currentMaterialIndex >= materials.Count) {
                currentMaterialIndex = 0;
            }
            currentMaterial = materials[currentMaterialIndex];
        }
    }
}
