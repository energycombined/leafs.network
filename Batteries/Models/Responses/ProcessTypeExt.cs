using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProcessTypeExt : ProcessType
    {

        public ProcessTypeExt(ProcessType e)
        {
            this.processTypeId = e.processTypeId;
            this.processType = e.processType;
            this.processDatabaseType = e.processDatabaseType;
            this.subcategory = e.subcategory;
        }
    }
}