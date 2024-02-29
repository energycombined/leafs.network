using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class BatteryComponent
    {
        public long batteryComponentId { get; set; }
        public int? fkExperiment { get; set; }
        public int? fkBatteryComponentType { get; set; }
        public int? step { get; set; }
        public long? fkStepMaterial { get; set; }
        public int? fkStepBatch { get; set; }
        [Required]
        public double? weight { get; set; }
        public int? fkFunction { get; set; }
        public int? fkMaterialType { get; set; }
        public int? fkStoredInType { get; set; }
        public int? orderInStep { get; set; }
        public long? fkCommercialType { get; set; }
        public Boolean? isComplete { get; set; }
        public Boolean? isSavedAsBatch { get; set; }
        public double? percentageOfActive { get; set; }
    }
}