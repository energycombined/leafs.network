using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.ProcessModels
{
    public class Mixing
    {
        public long mixingId { get; set; }
        public long? fkExperimentProcess { get; set; }
        public long? fkBatchProcess { get; set; }
        public int? fkEquipment { get; set; }
        
        //public int? rotationSpeed { get; set; }
        //public int? rotationTime { get; set; }
        //public string cupType { get; set; }
        //public string cupMaterial { get; set; }
        //public int? cupSize { get; set; }
        //public string containerType { get; set; }
        //public string containerMaterial { get; set; }
        //public int? containerSize { get; set; }
        //public int? programChannel { get; set; }

        public string comments { get; set; }
        public string label { get; set; }
        public DateTime? dateCreated { get; set; }

    }
}