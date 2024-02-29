using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class BatchContent
    {
        public long batchContentId { get; set; }
        public int? fkBatch { get; set; }
        public int? step { get; set; }
        public long? fkStepMaterial { get; set; }
        public int? fkStepBatch { get; set; }
        [Required]
        public double? weight { get; set; }
        public int? fkFunction { get; set; }
        public int? orderInStep { get; set; }
        public Boolean? isComplete { get; set; }
        public double? percentageOfActive { get; set; }

    }
}