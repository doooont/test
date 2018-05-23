using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Fishtail.PlayTheBall.GearPrototypes
{
    public class RedirectorTouchInput :
        MonoBehaviour, IPointerClickHandler
    {
        private Redirector _redirector;

        private void Awake()
        {
            _redirector = this.GetComponentInParent<Redirector>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _redirector.direction = this.GetNextDirection(_redirector.direction);
        }

        private Redirector.Direction GetNextDirection(Redirector.Direction dir)
        {
            var i = (int)dir;
            return (Redirector.Direction)(i == 3 ? 0 : i + 1);
        }
    }
}
