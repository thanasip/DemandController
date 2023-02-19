using ColossalFramework.UI;
using ICities;
using static DemandController.DemandControllerUIPanel;

namespace DemandController
{
    public class DemandController : LoadingExtensionBase, IUserMod
    {
        public static UICheckBox _buttonCheck;
        public static UICheckBox _shortcutCheck;

        public string Name => "Demand Controller";

        public string Description => "Allows fine-grained control of RCW demand";

        public void OnSettingsUI(UIHelperBase helper)
        {
            var config = Configuration<DemandControllerConfiguration>.Load();
            var buttonEnabled = config.ButtonEnabled ?? true;
            var shortcutEnabled = config.ShortcutEnabled ?? true;

            var masterGroup = helper.AddGroup("Settings");
            _buttonCheck = (UICheckBox)masterGroup.AddCheckbox("Button enabled", buttonEnabled, val =>
            {
                config.ButtonEnabled = val;
                SaveAndRefresh();
            });

            _shortcutCheck = (UICheckBox)masterGroup.AddCheckbox("Keyboard shortcut (Ctrl+Atl+D) enabled", shortcutEnabled, val =>
            {
                config.ShortcutEnabled = val;
                SaveAndRefresh();
            });
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            DemandControllerThreading.Init();
            DemandControllerExtension.Refresh(true);
        }
    }
}
