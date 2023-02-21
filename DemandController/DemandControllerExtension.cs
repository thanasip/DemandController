using ICities;

namespace DemandController
{
    public class DemandControllerExtension : DemandExtensionBase
    {
        private static bool Enabled;

        private static int ResidentialDemand;
        private static bool ResidentialEnabled;

        private static int CommercialDemand;
        private static bool CommercialEnabled;

        private static int WorkplaceDemand;
        private static bool WorkplaceEnabled;

        public override int OnCalculateResidentialDemand(int originalDemand) => Enabled && ResidentialEnabled
            ? SetResidentialDemand()
            : base.OnCalculateResidentialDemand(originalDemand);

        public override int OnCalculateCommercialDemand(int originalDemand) => Enabled && CommercialEnabled
            ? SetCommercialDemand()
            : base.OnCalculateCommercialDemand(originalDemand);

        public override int OnCalculateWorkplaceDemand(int originalDemand) => Enabled && WorkplaceEnabled 
            ? SetWorkplaceDemand()
            : base.OnCalculateWorkplaceDemand(originalDemand);

        public override int OnUpdateDemand(int lastDemand, int nextDemand, int targetDemand)
        {
            if (Enabled)
            {
                if (targetDemand == ResidentialDemand && ResidentialEnabled)
                    return SetResidentialDemand();
                if (targetDemand == CommercialDemand && CommercialEnabled)
                    return SetCommercialDemand();
                if (targetDemand == WorkplaceDemand && WorkplaceEnabled)
                    return SetWorkplaceDemand();
            }

            return base.OnUpdateDemand(lastDemand, nextDemand, targetDemand);
        }

        private int SetResidentialDemand()
        {
            var zoneMgr = ZoneManager.instance;
            zoneMgr.m_residentialDemand = ResidentialDemand;
            return ResidentialDemand;
        }

        private int SetCommercialDemand()
        {
            var zoneMgr = ZoneManager.instance;
            zoneMgr.m_commercialDemand = CommercialDemand;
            return CommercialDemand;
        }

        private int SetWorkplaceDemand()
        {
            var zoneMgr = ZoneManager.instance;
            zoneMgr.m_workplaceDemand = WorkplaceDemand;
            return WorkplaceDemand;
        }

        public static void Refresh()
        {
            var config = Configuration<DemandControllerConfiguration>.Load();
            Enabled = config.Enabled;

            ResidentialDemand = config.ResidentialDemand;
            ResidentialEnabled = config.ResidentialEnabled;

            CommercialDemand = config.CommercialDemand;
            CommercialEnabled = config.CommercialEnabled;

            WorkplaceDemand = config.WorkplaceDemand;
            WorkplaceEnabled = config.WorkplaceEnabled;
        }
    }
}
