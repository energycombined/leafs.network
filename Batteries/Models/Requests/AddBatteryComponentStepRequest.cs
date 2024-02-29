using Batteries.Models.Responses;
using Batteries.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models.Requests
{
    public class AddBatteryComponentStepRequest
    {
        public Boolean isSavedAsBatch;
        //[Required, ValidateCollection]
        public List<BatteryComponentExt> stepContent { get; set; }
        public List<dynamic> stepProcesses { get; set; }
        public MeasurementsExt measurements { get; set; }
        public int stepNumber { get; set; }        
    }
}