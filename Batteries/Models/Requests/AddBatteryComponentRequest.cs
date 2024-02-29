using Batteries.Models.Responses;
using Batteries.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models.Requests
{
    public class AddBatteryComponentRequest
    {
        public bool componentEmpty;
        public string componentType;
        public int userId;
        public int experimentId;
        public List<AddBatteryComponentStepRequest> componentStepsContentList { get; set; }
        public MeasurementsExt measurements { get; set; }
    }
}