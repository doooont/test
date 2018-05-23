using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 老虎机的视图控制组件。
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class SlotMachineView : MonoBehaviour
    {
        private Animator _animator;
        private SlotMachine _slotMachine;

        public void Awake()
        {
            _animator = this.GetComponent<Animator>();
            _slotMachine = this.GetComponentInParent<SlotMachine>();
            _slotMachine.onActivated.AddListener(this.OnActivated);
            _slotMachine.onDraw.AddListener(this.OnDraw);
            _slotMachine.onDeactivated.AddListener(this.OnDeactivate);
        }

        private void OnActivated() =>
            _animator.SetTrigger("Activate");

        private void OnDraw(SlotMachine.Luck luck)
        {
            int luckValue = 0;

            switch (luck) {
            case SlotMachine.Luck.Normal:
                luckValue = 1;
                break;
            case SlotMachine.Luck.Rare:
                luckValue = 2;
                break;
            case SlotMachine.Luck.SuperRare:
                luckValue = 3;
                break;
            }

            _animator.SetInteger("Luck", luckValue);
        }

        private void OnDeactivate()
        {
            // 老虎机默认只能使用一次，因此这里不触发 Deactivate，保留动画状态。
            // _animator.SetTrigger("Deactivate");
        }
    }
}
