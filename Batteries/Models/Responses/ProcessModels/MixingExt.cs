using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class MixingExt : Mixing
    {
        public string equipmentName { get; set; }
        public MixingExt(Mixing e)
        {
            if (e != null)
            {
                this.mixingId = e.mixingId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipment = e.fkEquipment;
                this.comments = e.comments;
                this.label = e.label;
                this.dateCreated = e.dateCreated;

                //this.rotationSpeed = e.rotationSpeed;
                //this.rotationTime = e.rotationTime;
                //this.cupType = e.cupType;
                //this.cupMaterial = e.cupMaterial;
                //this.cupSize = e.cupSize;
                //this.containerType = e.containerType;
                //this.containerMaterial = e.containerMaterial;
                //this.containerSize = e.containerSize;
                //this.programChannel = e.programChannel;
            }
        }
    }
}