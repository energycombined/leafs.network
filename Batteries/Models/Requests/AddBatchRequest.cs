using Batteries.Models.Responses;
using Batteries.Models.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Models.Requests
{
    public class AddBatchRequest
    {
        public BatchExt batchInfo { get; set; }
        [Required, ValidateCollection]
        public List<BatchContentExt> batchContent { get; set; }
        [Required]
        public List<BatchProcessResponse> batchProcesses { get; set; }
        public MeasurementsExt measurements { get; set; }
        public int fkExperiment { get; set; }
    }
}