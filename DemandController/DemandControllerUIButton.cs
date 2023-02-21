using ColossalFramework.UI;
using UnityEngine;

namespace DemandController
{
    public class DemandControllerUIButton : UIButton
    {
        private DemandControllerConfiguration _config;
        private bool _dragging = false;

        public override void Start()
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
            name = "DCUIB";
            canFocus = false;
            enabled = true;
            autoSize = false;
            normalBgSprite = "AssetEditorItemBackgroundHovered";
            normalFgSprite = "InfoPanelRCIOIndicator";
            color = new Color32(180, 180, 180, 255);
            scaleFactor = 0.8f;
            focusedColor = new Color32(180, 180, 180, 255);
            hoveredColor = new Color32(250, 250, 250, 255);
            relativePosition = _config.ButtonPosition;
            size = new Vector3(32f, 32f);
            bringTooltipToFront = true;
            BringToFront();
            eventClicked += (s, e) =>
            {
                if (!_dragging)
                {
                    Debug.LogWarning("click!");
                    DemandControllerThreading.ToggleDemandControllerUI();
                }
                else
                {
                    Debug.LogWarning("stop drag!");
                    _config.ButtonPosition = relativePosition;
                    _dragging = false;
                    DemandController.SaveAndRefresh();
                }
            };

            var drag = AddUIComponent<UIDragHandle>();
            drag.size = new Vector3(32f, 32f);
            drag.relativePosition = Vector3.zero;
            drag.target = this;
            drag.tooltip = "Open Demand Controller";
            drag.eventDragStart += (s, e) =>
            {
                Debug.LogWarning("start drag!");
                _dragging = true;
            };

            if (!_config.ButtonEnabled) Hide();
        }
    }
}
