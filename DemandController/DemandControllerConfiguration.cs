namespace DemandController
{
    [ConfigurationPath("DemandController_Settings.xml")]
    public class DemandControllerConfiguration
    {
        public int? ResidentialDemand { get; set; } = 50;

        public int? CommercialDemand { get; set; } = 50;

        public int? WorkplaceDemand { get; set; } = 50;

        public bool? Enabled { get; set; } = true;
    }
}
