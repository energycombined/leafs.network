using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    public class ValidationHelper
    {
        public static bool IsModelValid(object model)
        {
            var validationContext = new ValidationContext(model, null, null);            
            return Validator.TryValidateObject(model, validationContext, null, true);
        }

        public static List<ValidationResult> IsModelValidWithErrors(object model)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults; 
        }  
    }
}