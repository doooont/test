using System;
using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishtail.PlayTheBall.Player
{
    /// <summary>
    /// 使用拖拽事件控制发射器装置的组件。
    /// </summary>
    [RequireComponent(typeof(Launcher))]
    public class LauncherDragInput
        : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Tooltip("激活装置所需要的拖拽距离。")]
        public float activationRadius = 3f;
        [Tooltip("将拖拽距离转换成冲击力时使用的因数。")]
        public float impulseMultiplier = 1f;

        private Launcher _launcher;
        private bool _dragAvailable;

        private void Awake()
        {
            _launcher = this.GetComponent<Launcher>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_launcher.activated || !_launcher.targetRigidbody.IsSleeping()) {
                // 发射装置只有在球体静止时才能被唤醒。
                _dragAvailable = false;
                return;
            }

            _launcher.AwakeLauncher();
            _dragAvailable = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragAvailable) { return; }

            var hitPos = eventData.pointerCurrentRaycast.worldPosition;
            var diff = hitPos - transform.position;
            var magnitude = diff.magnitude;

            if (!_launcher.activated) {
                if (magnitude > activationRadius) {
                    _launcher.Activate();
                }
                else {
                    return;
                }
            }
            else if (magnitude < activationRadius) {
                _launcher.Deactivate();
                return;
            }

            // 用于计算冲击力的拖拽距离会减去激活装置所需要的最小距离。
            float dragDistance = magnitude - activationRadius;
            _launcher.SetImpulse(-diff.normalized * dragDistance * impulseMultiplier);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragAvailable) { return; }

            if (!_launcher.activated) {
                _launcher.StopLaunch();
                return;
            }
            _launcher.Launch();
        }
    }
}