using Batteries.Models.EquipmentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.EquipmentModels
{
    public class MillingBallMillExt : MillingBallMill
    {
        public string equipmentModelName { get; set; }
        public MillingBallMillExt(MillingBallMill e)
        {
            if (e != null)
            {
                this.settingsId = e.settingsId;
                this.fkExperimentProcess = e.fkExperimentProcess;
                this.fkBatchProcess = e.fkBatchProcess;
                this.fkEquipmentModel = e.fkEquipmentModel;
                this.ballPowderRatio = e.ballPowderRatio;
                this.millingSpeed = e.millingSpeed;
                this.millingTime = e.millingTime;
                this.restingTime = e.restingTime;
                this.loopCount = e.loopCount;
                this.cupVolume = e.cupVolume;
                this.cupMaterial = e.cupMaterial;
                this.ballsSize = e.ballsSize;
                this.ballsMaterial = e.ballsMaterial;
                this.amountOfBalls = e.amountOfBalls;
                this.comment = e.comment;
                this.label = e.label;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}