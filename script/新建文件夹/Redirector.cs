using System;

using UnityEngine;
using UnityEngine.Events;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 变向器能够将物体按照预设的方位射出。
    /// </summary>
    public class Redirector : BaseEmitter
    {
        public enum Direction
        {
            Front,
            Right,
            Back,
            Left
        }

        [Serializable]
        public class DirectionChangedEvent : UnityEvent<Direction> { }

        [Header("Pivot")]

        [Tooltip("四个方向上的发射点。")]
        public Transform emitPivots;

        [Tooltip("默认的发射方向。")]
        public Direction defaultDirection;

        public DirectionChangedEvent onDirectionChanged;

        /// <summary>
        /// 发射物体时使用的方向。
        /// </summary>
        public Direction direction {
            get {
                return _direction;
            }
            set {
                _direction = value;
                onDirectionChanged.Invoke(_direction);
            }
        }

        private Direction _direction;

        private void Start()
        {
            direction = defaultDirection;
        }

        protected override Transform GetEmitPivot() =>
            emitPivots.Find(direction.ToString());
    }
}
