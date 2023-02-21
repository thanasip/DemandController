using ColossalFramework.UI;
using ICities;

namespace DemandController
{
    public class DemandController : LoadingExtensionBase, IUserMod
    {
        private DemandControllerConfiguration _config;

        private DemandControllerUIButton _button;

        public string Name => "Demand Controller";

        public string Description => "Allows fine-grained control of RCW demand";

        public void OnSettingsUI(UIHelperBase helper)
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
            var buttonEnabled = _config.ButtonEnabled;
            var shortcutEnabled = _config.ShortcutEnabled;

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

        public static void SaveAndRefresh()
        {
            Configuration<DemandControllerConfiguration>.Save();
            DemandControllerExtension.Refresh();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            var view = UIView.GetAView();
            var panel = view.AddUIComponent(typeof(DemandControllerUIPanel));
            _button = (DemandControllerUIButton)view.AddUIComponent(typeof(DemandControllerUIButton));
            panel.Hide();
        }

        private void ToggleDemandControllerButton()
        {
            if (!_button.isVisible)
                _button.Show();
            else
                _button.Hide();
        }
    }
}
