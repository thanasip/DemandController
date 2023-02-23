using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace DemandController
{
    public class DemandController : LoadingExtensionBase, IUserMod
    {
        private static DemandControllerConfiguration _config;

        private static DemandControllerUIButton _button;

        public static bool _loaded = false;

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

            masterGroup.AddSpace(3);

            masterGroup.AddCheckbox("Keyboard shortcut (Ctrl+Atl+D) enabled", shortcutEnabled, val =>
            {
                _config.ShortcutEnabled = val;
                SaveAndRefresh();
            });

            masterGroup.AddSpace(9);

            masterGroup.AddButton("Reset Button/Panel position", () =>
            {
                if (!_loaded)
                {
                    var panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
                    panel.SetMessage("Error!", "Can't reset while level is not loaded", false);
                }
                else
                {
                    ConfirmPanel.ShowModal("Warning!", "Reset Button and Panel positions?", (sender, result) =>
                    {
                        if (result == 1)
                        {
                            ResetUI();
                        }
                    });
                }
            });
        }

        public static void SaveAndRefresh()
        {
            Configuration<DemandControllerConfiguration>.Save();
            DemandControllerExtension.Refresh();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame)
            {
                Init();
            }
        }

        public static void Init()
        {
            var view = UIView.GetAView();
            var panel = view.AddUIComponent(typeof(DemandControllerUIPanel));
            _button = (DemandControllerUIButton)view.AddUIComponent(typeof(DemandControllerUIButton));
            panel.Hide();
            SaveAndRefresh();
            _loaded = true;
        }

        public static void DestroyComponents()
        {
            var panel = (DemandControllerUIPanel)UIView.Find("DemandControllerUIPanel");
            UIView.Destroy(panel);
            UIView.Destroy(_button);
        }

        public static void ResetUI()
        {
            try
            {
                Debug.LogWarning("RESET!");
                _config.ButtonPosition = new Vector3(168f, 200f);
                _config.PanelPosition = new Vector3(200f, 200f);
                Configuration<DemandControllerConfiguration>.Save();
                DestroyComponents();
                Init();
                DemandControllerExtension.Refresh();
            } 
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void ToggleDemandControllerButton()
        {
            try
            {
                if (!_button.isVisible)
                    _button.Show();
                else
                    _button.Hide();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}
