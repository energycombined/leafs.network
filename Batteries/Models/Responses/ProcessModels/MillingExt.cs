using Batteries.Models.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses.ProcessModels
{
    public class MillingExt : Milling
    {
        public string equipmentName { get; set; }
        public MillingExt(Milling e)
        {
            this.millingId = e.millingId;
            this.fkExperimentProcess = e.fkExperimentProcess;
            this.fkBatchProcess = e.fkBatchProcess;
            this.fkEquipment = e.fkEquipment;
            this.comments = e.comments;
            this.label = e.label;
            this.dateCreated = e.dateCreated;

            //this.ballMillCupsSize = e.ballMillCupsSize;
            //this.ballMillCupsMaterial = e.ballMillCupsMaterial;
            //this.ballMillCupsVendor = e.ballMillCupsVendor;
            //this.ballsSize = e.ballsSize;
            //this.ballsMaterial = e.ballsMaterial;
            //this.ballsAmount = e.ballsAmount;
            //this.millingSpeedRpm = e.millingSpeedRpm;
            //this.restingTime = e.restingTime;
            //this.loopCount = e.loopCount;
            //this.ballPowderRatio = e.ballPowderRatio;
            

            //this.grBalls = e.grBalls;
            //this.ballPowderRatio = e.ballPowderRatio;
            //this.rpm = e.rpm;
            //this.pauseTime = e.pauseTime;
            //this.repetition = e.repetition;
        }
    }
}