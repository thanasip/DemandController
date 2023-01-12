using ICities;

namespace DemandController
{
    public class DemandController : IUserMod
    {
        public string Name => "Demand Controller";

        public string Description => "Allows fine-grained control of RCW demand";

        public void OnSettingsUI(UIHelperBase helper)
        {
            var config = Configuration<DemandControllerConfiguration>.Load();

            helper.AddCheckbox("Enabled", config.Enabled ?? true, val =>
            {
                config.Enabled = val;
                SaveAndRefresh();
            });

            helper.AddSlider("Residential Demand", 0f, 100f, 1f, config.ResidentialDemand ?? 50f, val =>
            {
                config.ResidentialDemand = (int)val;
                SaveAndRefresh();
            });

            helper.AddSlider("Commercial Demand", 0f, 100f, 1f, config.CommercialDemand ?? 50f, val =>
            {
                config.CommercialDemand = (int)val;
                SaveAndRefresh();
            });

            helper.AddSlider("Workplace Demand", 0f, 100f, 1f, config.WorkplaceDemand ?? 50f, val =>
            {
                config.WorkplaceDemand = (int)val;
                SaveAndRefresh();
            });
        }

        private void SaveAndRefresh()
        {
            Configuration<DemandControllerConfiguration>.Save();
            DemandControllerExtension.Refresh();
        }
    }
}
