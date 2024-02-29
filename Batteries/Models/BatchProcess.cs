using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class BatchProcess
    {
        public long batchProcessId { get; set; }
        [Required]
        public int? fkBatch { get; set; }
        [Required]
        public int? step { get; set; }
        //public int? fkProcessType { get; set; }
        [Required]
        public int? processOrderInStep { get; set; }
        public int? fkProcess { get; set; }
        public string label { get; set; }

    }
}