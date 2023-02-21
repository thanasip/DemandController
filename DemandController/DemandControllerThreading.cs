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
        private DemandControllerConfiguration _config;
        private bool _lock = false;
        private bool _shortcutEnable = false;

        public override void OnCreated(IThreading threading)
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
            _shortcutEnable = _config.ShortcutEnabled ?? true;
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
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

        public static void ToggleDemandController()
        {
            var comp = UIView.Find("DemandControllerUIPanel", typeof(DemandControllerUIPanel));

            if (comp == null)
            {
                var view = UIView.GetAView();
                view.AddUIComponent(typeof(DemandControllerUIPanel));
            } else
            {
                UIView.DestroyImmediate(comp);
            }
        }
    }
}
