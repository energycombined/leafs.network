using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Requests
{
    public class AddExperimentRequest
    {
        public ExperimentExt experimentInfo { get; set; }
        public List<AddBatteryComponentRequest> batteryComponents { get; set; }
    }
}