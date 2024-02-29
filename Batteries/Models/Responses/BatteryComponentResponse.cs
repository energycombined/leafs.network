using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatteryComponentResponse
    {
        public string componentType;
        public bool isCommercialType;
        public List<BatteryComponentStepResponse> batteryComponentSteps { get; set; }
        public BatteryComponentCommercialTypeExt commercialType { get; set; }
        public MeasurementsExt measurements { get; set; }
    }
}