using Batteries.Models.Responses;
using Batteries.Models.Responses.ProcessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Requests
{
    public class AddSequenceRequest
    {
        public ProcessSequence sequenceInfo { get; set; }
        public List<ProcessRequest> sequenceProcesses { get; set; }

    }
}