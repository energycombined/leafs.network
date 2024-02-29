using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Batteries.Models
{
    [Serializable]
    public class User
    {
        [Required]
        public int userId { get; set; }
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public bool active { get; set; }
        public Role userRole { get; set; }
        public int? fkResearchGroup { get; set; }
    }
}