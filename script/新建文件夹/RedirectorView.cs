using System;

using UnityEngine;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    /// <summary>
    /// 变向器的视图控制组件。
    /// </summary>
    public class RedirectorView : BaseEmitterView
    {
        [Tooltip("激活方向上的材质的自发光颜色。")]
        public Color activeLightColor = Color.white;

        private Transform _lights;
        private Material _activeMat;
        private Color _inactiveColor;

        private const string kEmissionColorProp = "_EmissionColor";

        protected override void Awake()
        {
            base.Awake();

            _lights = transform.Find("Body");

            var redirector = emitter as Redirector;
            redirector.onDirectionChanged.AddListener(this.OnDirectionChanged);
        }
        
        private void OnDirectionChanged(Redirector.Direction direction)
        {
            _activeMat?.SetColor(kEmissionColorProp, _inactiveColor);

            var renderer = _lights.Find(direction.ToString()).GetComponent<Renderer>();
            _activeMat = renderer.material;

            _inactiveColor = _activeMat.GetColor(kEmissionColorProp);
            _activeMat.SetColor(kEmissionColorProp, activeLightColor);
        }
    }
}
