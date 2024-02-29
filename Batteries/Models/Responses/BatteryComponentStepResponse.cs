using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatteryComponentStepResponse
    {
        public int stepNumber;
        public bool isSavedAsBatch;
        public List<BatteryComponentExt> stepContent { get; set; }
        public List<ProcessResponse> stepProcesses { get; set; }
        public MeasurementsExt measurements { get; set; }

        //public List<ExperimentProcessExt> stepProcesses { get; set; }
        //public List<dynamic> stepProcesses { get; set; }
        //moze lista na ExperimentProcess a atrubutite da se zemaat so alpaca na view.

    }
}