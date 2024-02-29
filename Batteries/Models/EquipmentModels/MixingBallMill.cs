using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.EquipmentModels
{
    public class MixingBallMill
    {
        public long settingsId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipmentModel { get; set; }
        public double? ballPowderRatio { get; set; }
        public double? millingSpeed { get; set; }
        public double? millingTime { get; set; }
        public double? restingTime { get; set; }
        public int? loopCount { get; set; }
        public double? cupVolume { get; set; }
        public string cupMaterial { get; set; }
        public double? ballsSize { get; set; }
        public string ballsMaterial { get; set; }
        public int? amountOfBalls { get; set; }
        public string comment { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}