﻿using UnityEngine;

namespace DemandController
{
    [ConfigurationPath("DemandController_Settings.xml")]
    public class DemandControllerConfiguration
    {
        public int? ResidentialDemand { get; set; } = 50;

        public int? CommercialDemand { get; set; } = 50;

        public int? WorkplaceDemand { get; set; } = 50;

        public bool? Enabled { get; set; } = true;

        public bool? ResidentialEnabled { get; set; } = true;

        public bool? CommercialEnabled { get; set; } = true;

        public bool? WorkplaceEnabled { get; set; } = true;

        public bool? ButtonEnabled { get; set;} = true;

        public bool? ShortcutEnabled { get; set; } = true;

        public Vector3? ButtonPosition { get; set; } = new Vector3(200f, 200f);

        public Vector3? PanelPosition { get; set; } = new Vector3(200f, 200f);
    }
}
