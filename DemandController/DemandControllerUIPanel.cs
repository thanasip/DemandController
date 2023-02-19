using ColossalFramework.UI;
using UnityEngine;
using System;

namespace DemandController
{
    public class CategoryGroup
    {
        public UISlider Slider { get; set; }

        public UILabel Label { get; set; }
    }

    public class DemandControllerUIPanel : UIPanel
    {
        private DemandControllerConfiguration _config;
        private static  UICheckBox _buttonCheck;
        private static UICheckBox _shortcutCheck;

        public override void Start()
        {
            try
            {
                _config = Configuration<DemandControllerConfiguration>.Load();

                backgroundSprite = "GenericPanel";
                color = new Color32(40, 40, 40, 255);
                width = 360;
                autoFitChildrenVertically = true;
                autoLayout = true;
                autoLayoutDirection = LayoutDirection.Vertical;
                autoLayoutStart = LayoutStart.TopLeft;
                autoLayoutPadding = new RectOffset(0, 0, 0, 8);
                absolutePosition = _config.PanelPosition ?? new Vector3(200f, 200f); ;

                var toolbar = CreateToolbar(null, width, 0, 0, 0, 0, LayoutStart.TopLeft);
                var toolbarL = CreateToolbar(toolbar, width * (2f / 3f), 0, 8, 0, 0, LayoutStart.TopLeft);
                var toolbarR = CreateToolbar(toolbar, width / 3f, 4, 4, 0, 0, LayoutStart.TopRight);

                CreateCheckBox(toolbarL, new Vector2(32f, 32f),
                    "AchievementCheckedTrue", "AchievementCheckedFalse", "Enable/Disable Globally",
                    _config.Enabled ?? true,
                    (sender, args) =>
                    {
                        _config.Enabled = args;
                        SaveAndRefresh();
                    }
                );

                var title = toolbarL.AddUIComponent<UILabel>();
                title.text = "Demand Controller";
                title.padding = new RectOffset(0, 0, 8, 0);
                title.textScale = 1.25f;
                title.verticalAlignment = UIVerticalAlignment.Middle;

                var drag = toolbarR.AddUIComponent<UIDragHandle>();
                var dragSprite = drag.AddUIComponent<UISprite>();
                dragSprite.spriteName = "buttonresize";
                dragSprite.size = new Vector2(32f, 32f);
                drag.tooltip = "Drag to move window";
                drag.size = new Vector2(32f, 32f);
                drag.target = this;
                drag.eventClicked += (s, e) =>
                {
                    _config.PanelPosition = absolutePosition;
                    SaveAndRefresh();
                    Debug.LogWarning("finish panel drag!");
                };

                var close = CreateCloseButton(toolbarR);

                _buttonCheck = CreateCheckboxGroup("UI Button", "Enable/Disable the UI button",
                    _config.ButtonEnabled ?? true, "buttonCheck",
                    (sender, args) =>
                    {
                        _config.ButtonEnabled = args;
                        SaveAndRefresh();
                    }
                );

                _shortcutCheck = CreateCheckboxGroup("Shortcut (Ctrl+Alt+D)", "Enable/Disable the keyboard shortcut",
                    _config.ShortcutEnabled ?? true, "shortcutCheck",
                    (sender, args) =>
                    {
                        _config.ShortcutEnabled = args;
                        SaveAndRefresh();
                    }
                );

                var residentialComponents = CreateCategoryGroup("Residential", "Enable/Disable for Residential", 
                    _config.ResidentialEnabled ?? true, _config.ResidentialDemand ?? 50,
                    (sender, args) =>
                    {
                        _config.ResidentialEnabled = args;
                        SaveAndRefresh();
                    }
                );

                residentialComponents.Slider.eventValueChanged += (sender, args) =>
                {
                    _config.ResidentialDemand = (int)args;
                    residentialComponents.Label.text = $"{args}";
                    SaveAndRefresh();
                };

                var commercialComponents = CreateCategoryGroup("Commercial", "Enable/Disable for Commercial",
                    _config.CommercialEnabled ?? true, _config.CommercialDemand ?? 50,
                    (sender, args) =>
                    {
                        _config.CommercialEnabled = args;
                        SaveAndRefresh();
                    }
                );

                commercialComponents.Slider.eventValueChanged += (sender, args) =>
                {
                    _config.CommercialDemand = (int)args;
                    commercialComponents.Label.text = $"{args}";
                    SaveAndRefresh();
                };

                var workplaceComponents = CreateCategoryGroup("Workplace", "Enable/Disable for Workplace",
                    _config.WorkplaceEnabled ?? true, _config.WorkplaceDemand ?? 50,
                    (sender, args) =>
                    {
                        _config.WorkplaceEnabled = args;
                        SaveAndRefresh();
                    }
                );

                workplaceComponents.Slider.eventValueChanged += (sender, args) =>
                {
                    _config.WorkplaceDemand = (int)args;
                    workplaceComponents.Label.text = $"{args}";
                    SaveAndRefresh();
                };

            }
            catch (Exception ex) 
            {
                Debug.LogException(ex);
            }
        }

        public static void SaveAndRefresh()
        {
            Configuration<DemandControllerConfiguration>.Save();
            DemandControllerExtension.Refresh();
            var config = Configuration<DemandControllerConfiguration>.Load();
            _buttonCheck.isChecked = config.ButtonEnabled ?? true;
            _shortcutCheck.isChecked = config.ShortcutEnabled ?? true;
            DemandController._buttonCheck.isChecked = config.ButtonEnabled ?? true;
            DemandController._shortcutCheck.isChecked = config.ShortcutEnabled ?? true;
        }

        private UISlider CreateSlider(UIPanel parent, float startingValue, string name)
        {
            var slider = parent.AddUIComponent<UISlider>();
            slider.backgroundSprite = "ScrollbarTrack";
            slider.thumbObject = slider.AddUIComponent<UISprite>();
            ((UISprite)slider.thumbObject).spriteName = "SliderBudget";
            slider.thumbObject.size = new Vector2(16, 16);
            slider.name = $"{name}Slider";
            slider.size = new Vector2(300, 8);
            slider.minValue = 0f;
            slider.maxValue = 100f;
            slider.scrollWheelAmount = 5f;
            slider.stepSize = 1f;
            slider.value = startingValue;
            return slider;
        }

        private UILabel CreateSliderText (UIPanel parent, string text, string name)
        {
            var label = parent.AddUIComponent<UILabel>();
            label.text = text;
            label.name = $"{name}label";
            label.size = new Vector2(32, 16);
            return label;
        }

        private UIPanel CreateControlGroup(UIPanel parent, int left, int right, int top, int bottom, string name)
        {
            var panel = parent.AddUIComponent<UIPanel>();
            panel.name = $"{name}group";
            panel.size = new Vector2(width, 0);
            panel.autoFitChildrenVertically = true;
            panel.autoLayout = true;
            panel.autoLayoutDirection = LayoutDirection.Horizontal;
            panel.autoLayoutStart = LayoutStart.TopLeft;
            panel.autoLayoutPadding = new RectOffset(left, right, top, bottom);
            return panel;
        }

        private CategoryGroup CreateCategoryGroup(string title, string tooltip,
            bool checkDefault, float valDefault, PropertyChangedEventHandler<bool> checkFunc)
        {
            var panel = AddUIComponent<UIPanel>();
            panel.size = new Vector2(width, 0);
            panel.autoFitChildrenVertically = true;
            panel.backgroundSprite = "GenericPanel";
            panel.color = new Color32(60, 60, 60, 255);
            panel.autoLayout = true;
            panel.autoLayoutDirection = LayoutDirection.Vertical;
            panel.autoLayoutStart = LayoutStart.TopLeft;
            panel.autoLayoutPadding = new RectOffset(0, 0, 0, 0);

            var titlePanel = CreateControlGroup(panel, 4, 4, 4, 4, $"{title}title");
            var controlPanel = CreateControlGroup(panel, 8, 8, 4, 8, $"{title}control");

            CreateCheckBox(titlePanel, new Vector2(16f, 16f),
                "check-checked", "check-unchecked", tooltip,
                checkDefault, checkFunc
            );

            var sliderL = titlePanel.AddUIComponent<UILabel>();
            sliderL.text = title;

            var slider = CreateSlider(controlPanel, valDefault, title);
            var sliderN = CreateSliderText(controlPanel, $"{valDefault}", title);
            return new CategoryGroup { Slider = slider, Label = sliderN };
        }

        private UICheckBox CreateCheckboxGroup(string title, string tooltip,
            bool checkDefault, string name, PropertyChangedEventHandler<bool> checkFunc)
        {
            var panel = AddUIComponent<UIPanel>();
            panel.size = new Vector2(width, 0);
            panel.autoFitChildrenVertically = true;
            panel.backgroundSprite = "GenericPanel";
            panel.color = new Color32(60, 60, 60, 255);
            panel.autoLayout = true;
            panel.autoLayoutDirection = LayoutDirection.Vertical;
            panel.autoLayoutStart = LayoutStart.TopLeft;
            panel.autoLayoutPadding = new RectOffset(0, 0, 0, 0);

            var titlePanel = CreateControlGroup(panel, 4, 4, 4, 4, $"{title}title");

            var check = CreateCheckBox(titlePanel, new Vector2(16f, 16f),
                "check-checked", "check-unchecked", tooltip,
                checkDefault, checkFunc
            );

            var sliderL = titlePanel.AddUIComponent<UILabel>();
            sliderL.text = title;

            return check;
        }

        private UICheckBox CreateCheckBox (UIComponent parent, Vector2 size, string checkedSprite, 
            string uncheckedSprite, string tooltip, bool? startingValue, PropertyChangedEventHandler<bool> func, string name = null)
        {
            var checkbox = parent.AddUIComponent<UICheckBox>();
            checkbox.size = size;
            checkbox.tooltip = tooltip;
            var masterUncheckedSprite = checkbox.AddUIComponent<UISprite>();
            masterUncheckedSprite.spriteName = uncheckedSprite;
            masterUncheckedSprite.size = size;
            masterUncheckedSprite.relativePosition = Vector3.zero;
            checkbox.checkedBoxObject = masterUncheckedSprite.AddUIComponent<UISprite>();
            ((UISprite)checkbox.checkedBoxObject).spriteName = checkedSprite;
            checkbox.checkedBoxObject.size = size;
            checkbox.checkedBoxObject.relativePosition = Vector3.zero;
            checkbox.isChecked = startingValue ?? true;
            checkbox.clipChildren = true;
            checkbox.eventCheckChanged += func;
            if (!string.IsNullOrEmpty(name))
            {
                checkbox.name = name;
            }
            return checkbox;
        }

        private UIPanel CreateToolbar(UIPanel parent, float width, int left, int right, int top, int bottom, LayoutStart side)
        {
            UIPanel toolbar;

            if (parent == null)
            {
                toolbar = AddUIComponent<UIPanel>();
            } 
            else
            {
                toolbar = parent.AddUIComponent<UIPanel>();
            }

            toolbar.size = new Vector2(width, 32);
            toolbar.backgroundSprite = "Menubar";
            toolbar.autoLayout = true;
            toolbar.autoLayoutDirection = LayoutDirection.Horizontal;
            toolbar.autoLayoutStart = side;
            toolbar.autoLayoutPadding = new RectOffset(left, right, top, bottom);

            return toolbar;
        }

        private UIButton CreateCloseButton(UIPanel parent)
        {
            var close = parent.AddUIComponent<UIButton>();
            close.size = new Vector2(32f, 32f);
            close.focusedBgSprite = "buttonclose";
            close.focusedFgSprite = "buttonclose";
            close.hoveredBgSprite = "buttonclosehover";
            close.hoveredFgSprite = "buttonclosehover";
            close.normalBgSprite = "buttonclose";
            close.normalFgSprite = "buttonclose";
            close.disabledBgSprite = "buttonclosepressed";
            close.disabledFgSprite = "buttonclosepressed";
            close.pressedBgSprite = "buttonclosepressed";
            close.pressedFgSprite = "buttonclosepressed";
            close.tooltip = "Close window";
            close.eventClicked += (sender, args) =>
            {
                var view = UIView.GetAView();
                var comp = view.FindUIComponent("DemandControllerUIPanel", typeof(DemandControllerUIPanel));
                comp.ToggleOff();
            };

            return close;
        }
    }
}
