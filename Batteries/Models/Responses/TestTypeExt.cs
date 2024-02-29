using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class TestTypeExt : TestType
    {
        public TestTypeExt(TestType e = null)
        {
            if (e != null)
            {
                this.testTypeId = e.testTypeId;
                this.testType = e.testType;
                this.supportsGraphing = e.supportsGraphing;
                this.testTypeSubcategory = e.testTypeSubcategory;
            }
        }
    }
}