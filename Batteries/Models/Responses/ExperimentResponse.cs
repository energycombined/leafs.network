using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ExperimentResponse
    {
        public ExperimentExt experimentInfo { get; set; }
        public List<BatteryComponentResponse> batteryComponents { get; set; }
    }
}