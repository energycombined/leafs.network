using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.EquipmentModels
{
    public class MixingPlanetaryMixer
    {
        public long settingsId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipmentModel { get; set; }
        public double? containerDiameterSize { get; set; }
        public int? amountOfContainers { get; set; }
        public int? programChannel { get; set; }
        public Boolean? manual { get; set; }
        public double? rotationSpeed { get; set; }
        public double? rotationTime { get; set; }
        public double? restTime { get; set; }
        public string comment { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}