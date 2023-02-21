using ColossalFramework.UI;
using ICities;

namespace DemandController
{
    public class DemandController : LoadingExtensionBase, IUserMod
    {
        private DemandControllerConfiguration _config;
        public string Name => "Demand Controller";

        public string Description => "Allows fine-grained control of RCW demand";

        public void OnSettingsUI(UIHelperBase helper)
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
            var buttonEnabled = _config.ButtonEnabled ?? true;
            var shortcutEnabled = _config.ShortcutEnabled ?? true;

            var masterGroup = helper.AddGroup("Settings");
            masterGroup.AddCheckbox("Button enabled", buttonEnabled, val =>
            {
                _config.ButtonEnabled = val;
                ToggleDemandControllerButton();
                SaveAndRefresh();
            });

            masterGroup.AddCheckbox("Keyboard shortcut (Ctrl+Atl+D) enabled", shortcutEnabled, val =>
            {
                _config.ShortcutEnabled = val;
                SaveAndRefresh();
            });
        }

        private void ToggleDemandControllerButton()
        {
            var comp = UIView.Find("DCUIB");

            if (comp == null)
            {
                var view = UIView.GetAView();
                view.AddUIComponent(typeof(DemandControllerUIButton));
            }
            else
            {
                UIView.DestroyImmediate(comp);
            }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
            var view = UIView.GetAView();
            view.AddUIComponent(typeof(DemandControllerUIButton));
            view.AddUIComponent(typeof(DemandControllerUIPanel));
        }

        public static void SaveAndRefresh()
        {
            Configuration<DemandControllerConfiguration>.Save();
            DemandControllerExtension.Refresh();
        }
    }
}
