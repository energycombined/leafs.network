using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class AtomicLayerDepositionExt : AtomicLayerDeposition
    {
        public string equipmentName { get; set; }

        public AtomicLayerDepositionExt(AtomicLayerDeposition e)
        {
            if (e != null)
            {
                this.atomicLayerDepositionId = e.atomicLayerDepositionId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.thickness = e.thickness;
                this.temperature = e.temperature;
                this.pressure = e.pressure;
                this.gas = e.gas;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}