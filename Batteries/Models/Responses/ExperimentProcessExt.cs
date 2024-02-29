using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ExperimentProcessExt : ExperimentProcess
    {
        public string batteryComponentType { get; set; }
        public int? fkProcessType { get; set; }
        public string processType { get; set; }
        public string processDatabaseType { get; set; }
        public int? fkEquipment { get; set; }
        public string equipmentName { get; set; }
        public string subcategory { get; set; }
        public int? equipmentModelId { get; set; }
        public string equipmentModelName { get; set; }
        public string modelBrand { get; set; }
        public ExperimentProcessExt (ExperimentProcess e = null)
        {
            if (e != null)
            {
                this.fkExperiment = e.fkExperiment;
                this.fkBatteryComponentType = e.fkBatteryComponentType;
                this.step = e.step;
                //this.fkProcessType = e.fkProcessType;
                this.experimentProcessId = e.experimentProcessId;
                this.processOrderInStep = e.processOrderInStep;
                this.isComplete = e.isComplete;
                this.label = e.label;
               // this.fkEquipment = e.fkEquipment;
                this.dateCreated = e.dateCreated;
                this.fkProcess = e.fkProcess;
            }
        }
    }
}