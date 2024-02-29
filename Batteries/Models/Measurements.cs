using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Measurements
    {
        public long measurementsId { get; set; }
        public int? fkMeasurementLevelType { get; set; }
        public int? fkExperiment { get; set; }
        public int? fkBatch { get; set; }
        public int? fkBatteryComponentType { get; set; }
        public int? stepId { get; set; }
        public long? fkBatteryComponentContent { get; set; }
        public long? fkBatchContent { get; set; }
        public double? measuredTime { get; set; }
        public double? measuredWidth { get; set; }
        public double? measuredLength { get; set; }
        public double? measuredConductivity { get; set; }
        public double? measuredThickness { get; set; }
        public double? measuredWeight { get; set; }
    }
}

