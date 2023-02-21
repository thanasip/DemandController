using ICities;

namespace DemandController
{
    public class DemandControllerExtension : DemandExtensionBase
    {
        private static bool Enabled;

        private static int  ResidentialDemand;
        private static bool ResidentialEnabled;

        private static int  CommercialDemand;
        private static bool CommercialEnabled;

        private static int  WorkplaceDemand;
        private static bool WorkplaceEnabled;

        public override void OnCreated(IDemand demand)
        {
            Refresh();
        }

        public override int OnCalculateResidentialDemand(int originalDemand) => Enabled && ResidentialEnabled
            ? ResidentialDemand 
            : base.OnCalculateResidentialDemand(originalDemand);

        public override int OnCalculateCommercialDemand(int originalDemand) => Enabled && CommercialEnabled
            ? CommercialDemand 
            : base.OnCalculateCommercialDemand(originalDemand);

        public override int OnCalculateWorkplaceDemand(int originalDemand) => Enabled && WorkplaceEnabled
            ? WorkplaceDemand 
            : base.OnCalculateWorkplaceDemand(originalDemand);

        public override int OnUpdateDemand(int lastDemand, int nextDemand, int targetDemand)
        {
            if (Enabled)
            {
                if (targetDemand == ResidentialDemand && ResidentialEnabled)
                    return ResidentialDemand;

                if (targetDemand == CommercialDemand && CommercialEnabled)
                    return CommercialDemand;

                if (targetDemand == WorkplaceDemand && WorkplaceEnabled)
                    return WorkplaceDemand;
            }

            return base.OnUpdateDemand(lastDemand, nextDemand, targetDemand);
        }

        public static void Refresh()
        {
            var config = Configuration<DemandControllerConfiguration>.Load();
            Enabled = config.Enabled ?? false;

            ResidentialDemand = config.ResidentialDemand ?? 50;
            ResidentialEnabled = config.ResidentialEnabled ?? true;

            CommercialDemand = config.CommercialDemand ?? 50;
            CommercialEnabled = config.CommercialEnabled ?? true;

            WorkplaceDemand = config.WorkplaceDemand ?? 50;
            WorkplaceEnabled = config.WorkplaceEnabled ?? true;
        }
    }
}
