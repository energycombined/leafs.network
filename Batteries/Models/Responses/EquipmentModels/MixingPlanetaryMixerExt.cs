using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class MixingPlanetaryMixerExt : MixingPlanetaryMixer
    {
        public string equipmentModelName { get; set; }
        public MixingPlanetaryMixerExt(MixingPlanetaryMixer e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.containerDiameterSize = e.containerDiameterSize;
                this.amountOfContainers = e.amountOfContainers;
                this.programChannel = e.programChannel;
                this.manual = e.manual;
                this.rotationSpeed = e.rotationSpeed;
                this.rotationTime = e.rotationTime;
                this.restTime = e.restTime;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}