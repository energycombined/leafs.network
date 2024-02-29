using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class CoPrecipitationExt : CoPrecipitation
    {
        public string equipmentName { get; set; }

        public CoPrecipitationExt(CoPrecipitation e)
        {
            if (e != null)
            {
                this.coPrecipitationId = e.coPrecipitationId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.atmosphere = e.atmosphere;
                this.pressure = e.pressure;
                this.temperature = e.temperature;
                this.time = e.time;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}