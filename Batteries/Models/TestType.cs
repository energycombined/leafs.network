using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class TestType
    {
        public int testTypeId { get; set; }
        public string testType { get; set; }
        public bool supportsGraphing { get; set; }
        public string testTypeSubcategory { get; set; }
    }
}