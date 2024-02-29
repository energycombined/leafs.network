using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class FiltratingExt : Filtrating
    {
        public string equipmentName { get; set; }

        public FiltratingExt(Filtrating e)
        {
            if (e != null)
            {
                this.filtratingId = e.filtratingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.filterWater = e.filterWater;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

            }
        }
    }
}