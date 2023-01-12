using ICities;

namespace DemandController
{
    public class DemandControllerExtension : DemandExtensionBase
    {
        private static DemandControllerConfiguration _config;

        public override void OnCreated(IDemand demand)
        {
            Refresh();
        }

        public override int OnCalculateResidentialDemand(int originalDemand)
        {
            if (_config.Enabled ?? false)
                return _config.ResidentialDemand ?? 50;

            return base.OnCalculateResidentialDemand(originalDemand);
        }

        public override int OnCalculateCommercialDemand(int originalDemand)
        {
            if (_config.Enabled ?? false)
                return _config.CommercialDemand ?? 50;

            return base.OnCalculateCommercialDemand(originalDemand);
        }

        public override int OnCalculateWorkplaceDemand(int originalDemand)
        {
            if (_config.Enabled ?? false)
                return _config.WorkplaceDemand ?? 50;

            return base.OnCalculateWorkplaceDemand(originalDemand);
        }

        public override int OnUpdateDemand(int lastDemand, int nextDemand, int targetDemand)
        {
            if (_config.Enabled ?? false)
            {
                if (targetDemand == _config.ResidentialDemand)
                    return _config.ResidentialDemand ?? 50;

                if (targetDemand == _config.CommercialDemand)
                    return _config.CommercialDemand ?? 50;

                if (targetDemand == _config.WorkplaceDemand)
                    return _config.WorkplaceDemand ?? 50;
            }

            return base.OnUpdateDemand(lastDemand, nextDemand, targetDemand);
        }

        public static void Refresh()
        {
            _config = Configuration<DemandControllerConfiguration>.Load();
        }
    }
}
