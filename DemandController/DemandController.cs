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
            var enabled = config.Enabled ?? true;

            var residentialDemand = config.ResidentialDemand ?? 50f;
            var residentialEnabled = config.ResidentialEnabled ?? true;

            var commercialDemand = config.CommercialDemand ?? 50f;
            var commercialEnabled = config.CommercialEnabled ?? true;

            var workplaceDemand = config.WorkplaceDemand ?? 50f;
            var workplaceEnabled = config.WorkplaceEnabled ?? true;

            var masterGroup = helper.AddGroup("Master settings");
            masterGroup.AddCheckbox("Master Enable", enabled, val =>
            {
                config.Enabled = val;
                SaveAndRefresh();
            });

            var resGroup = helper.AddGroup("Residential Settings");
            resGroup.AddSlider("Residential Demand", 0f, 100f, 1f, residentialDemand, val =>
            {
                config.ResidentialDemand = (int)val;
                SaveAndRefresh();
            });
            resGroup.AddCheckbox("Residential Enable", residentialEnabled, val =>
            {
                config.ResidentialEnabled = val;
                SaveAndRefresh();
            });

            var commGroup = helper.AddGroup("Commercial Settings");
            commGroup.AddSlider("Commercial Demand", 0f, 100f, 1f, commercialDemand, val =>
            {
                config.CommercialDemand = (int)val;
                SaveAndRefresh();
            });
            commGroup.AddCheckbox("Commercial Enable", commercialEnabled, val =>
            {
                config.CommercialEnabled = val;
                SaveAndRefresh();
            });

            var workGroup = helper.AddGroup("Workplace Settings");
            workGroup.AddSlider("Workplace Demand", 0f, 100f, 1f, workplaceDemand, val =>
            {
                config.WorkplaceDemand = (int)val;
                SaveAndRefresh();
            });
            workGroup.AddCheckbox("Workplace Enable", workplaceEnabled, val =>
            {
                config.WorkplaceEnabled = val;
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
