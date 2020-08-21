﻿
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        public RequiredIfNotAttribute(string dependentProperty, object dependentValue) : base(dependentProperty, Operator.NotEqualTo, dependentValue) { }
    }
}
