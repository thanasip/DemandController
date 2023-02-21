using ColossalFramework.UI;
using UnityEngine;

namespace DemandController
{
    public  class DemandControllerUIButton : UIPanel
    {
        private bool _dragging = false;
        UIView _view;
        UIPanel _zone;
        UIDragHandle _drag;
        DemandControllerConfiguration _config;

        public override void Start()
        {
            _config = Configuration<DemandControllerConfiguration>.Load();

            /*DragEventHandler startDragDelegate = (s, e) =>
            {
                Debug.LogWarning("start drag!");
                _dragging = true;
            };

            MouseEventHandler clickDelegate = (s, e) =>
             {
                 if (!_dragging)
                 {
                     Debug.LogWarning("click!");
                     DemandControllerThreading.ToggleDemandController();
                 }
                 else
                 {
                     Debug.LogWarning("stop drag!");
                     _config.ButtonPosition = _zone.relativePosition;
                     DemandController.SaveAndRefresh();
                     _dragging = false;
                 }
             };*/

            _view = UIView.GetAView();
            Debug.LogWarningFormat("w: {0} h: {1}", _view.fixedWidth, _view.fixedHeight);

            _zone = (UIPanel)_view.AddUIComponent(typeof(UIPanel));
            _zone.name = "DCUIB";
            _zone.backgroundSprite = "InfoPanelRCIOIndicator";
            _zone.relativePosition = _config.ButtonPosition ?? new Vector3(200f, 200f);
            _zone.size = new Vector3(32f, 32f);
            _zone.isVisible = true;
            /*_drag = _zone.AddUIComponent<UIDragHandle>();
            _drag.target = _zone;
            _drag.relativePosition = new Vector3(0f, 0f);
            _drag.size = _zone.size;
            _drag.tooltip = "Demand Controller";
            _drag.eventClicked += clickDelegate;
            _drag.eventDragStart += startDragDelegate;*/
        }
    }
}
