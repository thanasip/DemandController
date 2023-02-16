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

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        { 
            if (
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
                Input.GetKeyUp(KeyCode.D) &&
                !_lock
            )
            {
                _lock = true;

                var view = UIView.GetAView();
                var comp = view.FindUIComponent("DemandControllerUIPanel", typeof(DemandControllerUIPanel));

                if (comp == null)
                {
                    comp = view.AddUIComponent(typeof(DemandControllerUIPanel));
                    comp.ToggleOff();
                }
                else
                {
                    Debug.LogWarning(comp);
                }

                if (!comp.isVisible)
                    comp.ToggleOn();
                else
                    comp.ToggleOff();

            }
            else
            {
                _lock = false;
            }
        }
    }
}
