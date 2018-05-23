using System;
using System.Collections;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 能将碰撞到自身的物体从前方发射的装置。
    /// </summary>
    public class Emitter : BaseEmitter
    {
        [Header("Pivot")]
        [Tooltip("发射物体时使用的位置。")]
        public Transform emitPivot;

        protected override Transform GetEmitPivot() => emitPivot;
    }
}
