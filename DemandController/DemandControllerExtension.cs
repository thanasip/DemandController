using ColossalFramework.UI;
using ICities;
using UnityEngine;

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


        public override int OnCalculateResidentialDemand(int originalDemand)
        {
            if (Enabled && ResidentialEnabled)
            {
                var zoneMgr = ZoneManager.instance;
                zoneMgr.m_residentialDemand = ResidentialDemand;
                return ResidentialDemand;
            } 
            else
            {
                return base.OnCalculateResidentialDemand(originalDemand);
            }
        }

        public override int OnCalculateCommercialDemand(int originalDemand)
        {
            if (Enabled && CommercialEnabled)
            {
                var zoneMgr = ZoneManager.instance;
                zoneMgr.m_commercialDemand = CommercialDemand;
                return CommercialDemand;
            }
            else
            {
                return base.OnCalculateCommercialDemand(originalDemand);
            }
        }

        public override int OnCalculateWorkplaceDemand(int originalDemand)
        {
            if (Enabled && WorkplaceEnabled)
            {
                var zoneMgr = ZoneManager.instance;
                zoneMgr.m_workplaceDemand = WorkplaceDemand;
                return WorkplaceDemand;
            }
            else
            {
                return base.OnCalculateWorkplaceDemand(originalDemand);
            }
        }

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
