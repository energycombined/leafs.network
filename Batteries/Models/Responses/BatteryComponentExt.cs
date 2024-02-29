using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatteryComponentExt : BatteryComponent
    {
        public string batteryComponentType { get; set; }
        public string materialName { get; set; }
        public string batchSystemLabel { get; set; }
        public string batchPersonalLabel { get; set; }
        public string measurementUnitName { get; set; }
        public string measurementUnitSymbol { get; set; }
        public string chemicalFormula { get; set; }

        public string materialLabel { get; set; }
        public string description { get; set; }
        public string storedInType { get; set; }
        public string price { get; set; }
        public string bulkPrice { get; set; }
        public string materialFunction { get; set; }
        public double? batchOutput { get; set; }


        public MeasurementsExt measurements { get; set; }
        public BatteryComponentExt(BatteryComponent e = null)
        {
            if (e != null)
            {
                this.batteryComponentId = e.batteryComponentId;
                this.fkExperiment = e.fkExperiment;
                this.fkBatteryComponentType = e.fkBatteryComponentType;
                this.step = e.step;
                this.fkStepMaterial = e.fkStepMaterial;
                this.fkStepBatch = e.fkStepBatch;
                this.weight = e.weight;
                this.fkFunction = e.fkFunction;
                this.fkMaterialType = e.fkMaterialType;
                this.fkStoredInType = e.fkStoredInType;
                this.orderInStep = e.orderInStep;
                this.fkCommercialType = e.fkCommercialType;
                this.isComplete = e.isComplete;
                this.isSavedAsBatch = e.isSavedAsBatch;
                this.percentageOfActive = e.percentageOfActive;
            }
        }
    }
}