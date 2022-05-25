using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.AspNetCore.Validation.TestUtils
{
    internal class TestValidator
    {
        public static bool TryValidateObject(object model, out ICollection<ValidationResult>? results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();
            var result = Validator.TryValidateObject(model, context, results, true);
            return result;
        }
    }
}
