using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace DemandController
{
    public static class UIComponentExtensions
    {
        public static void ToggleOn(this UIComponent comp)
        {
            comp.isEnabled = true;
            comp.isVisible = true;
        }

        public static void ToggleOff(this UIComponent comp)
        {
            comp.isEnabled = false;
            comp.isVisible = false;
        }
    }

    public class DemandControllerThreading : ThreadingExtensionBase
    {
        private bool _lock = false;
        private static bool _shortcutEnable = false;
        private static DemandControllerConfiguration _config;
        private static UIComponent _panel;
        private static UIView _view;

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            _shortcutEnable = _config.ShortcutEnabled ?? true;

            if (
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
                Input.GetKeyUp(KeyCode.D) &&
                !_lock && _shortcutEnable

            )
            {
                _lock = true;
                ToggleDemandController();
            }
            else
            {
                _lock = false;
            }
        }

        public static void Init()
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
            _view = UIView.GetAView();
            _view.AddUIComponent(typeof(DemandControllerUIButton));
            _panel = _view.AddUIComponent(typeof(DemandControllerUIPanel));
            _panel.ToggleOff();
            _config.ScreenHeight = _view.fixedHeight;
            Configuration<DemandControllerConfiguration>.Save();
        }

        public static void ToggleDemandController()
        {
            var comp = _view.FindUIComponent("DemandControllerUIPanel", typeof(DemandControllerUIPanel));

            if (!comp.isVisible)
                comp.ToggleOn();
            else
                comp.ToggleOff();
        }
    }
}
