using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace DemandController
{
    public class DemandControllerThreading : ThreadingExtensionBase
    {
        private DemandControllerConfiguration _config;

        public override void OnCreated(IThreading threading)
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
        }

        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        { 
            if (
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) &&
                Input.GetKeyUp(KeyCode.D) &&
                (_config.ShortcutEnabled)
            )
            {
                ToggleDemandControllerUI();
            }
        }

        public static void ToggleDemandControllerUI()
        {
            var comp = GetDemandControllerPanel();

            if (comp != null)
            {
                if (!comp.isVisible)
                    comp.Show();
                else
                    comp.Hide();
            }
        }

        private static UIComponent GetDemandControllerPanel()
        {
            var view = UIView.GetAView();
            return view.FindUIComponent("DemandControllerUIPanel", typeof(DemandControllerUIPanel));
        }
    }
}
