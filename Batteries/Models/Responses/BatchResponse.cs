using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class BatchResponse
    {
        public BatchExt batchInfo { get; set; }
        public List<BatchContentExt> batchContent { get; set; }
        public List<BatchProcessResponse> batchProcesses { get; set; }
        public MeasurementsExt measurements { get; set; }
    }
}