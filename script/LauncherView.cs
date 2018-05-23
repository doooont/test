using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.Player
{
    /// <summary>
    /// LauncherView 为发射装置提供实际的显示效果。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class LauncherView : MonoBehaviour
    {
        [Tooltip("根据冲击力大小决定箭头显示偏移时使用的因数。")]
        public float arrowOffsetMultiplier = 0.1f;

        private Animator _animator;
        private Launcher _launcher;

        private Transform _arrow;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            _launcher = this.GetComponentInParent<Launcher>();

            _launcher.onAwake.AddListener(this.OnAwake);
            _launcher.onActivate.AddListener(this.OnActivate);
            _launcher.onDeactivate.AddListener(this.OnDeactivate);
            _launcher.onLaunch.AddListener(this.OnLaunch);
            _launcher.onStop.AddListener(this.OnStop);

            // 设置装置视图为根节点对象以避免其收到球体运动的影响。
            transform.parent = null;
            _arrow = transform.Find("Arrow");
        }

        private void OnAwake()
        {
            transform.position = _launcher.transform.position;
            _animator.SetTrigger("Awake");
        }

        private void OnActivate() =>
            _animator.SetTrigger("Activate");

        private void OnDeactivate() =>
            _animator.SetTrigger("Deactivate");

        private void OnLaunch() =>
            _animator.SetTrigger("Launch");

        private void OnStop() =>
            _animator.SetTrigger("Stop");

        private void FixedUpdate()
        {
            if (!_launcher.awaked) { return; }

            // 只有在动画处在“已激活”或“激活中”状态时才刷新箭头朝向。
            // 当弹射方向骤变时，这种做法可以获得更加美观的效果。
            var currentState = _animator.GetCurrentAnimatorStateInfo(0);
            if (!currentState.IsName("Active") && !currentState.IsName("Activate")) {
                return;
            }

            var newPos = _launcher.impulseDirection
                * _launcher.currentImpulse / _launcher.maximumImpulse
                * arrowOffsetMultiplier;
            newPos.y = _arrow.localPosition.y;

            _arrow.forward = _launcher.impulseDirection;
            _arrow.localPosition = newPos;
        }
    }
}
