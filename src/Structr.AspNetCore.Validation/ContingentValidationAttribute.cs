using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ContingentValidationAttribute : ModelAwareValidationAttribute
    {
        public string DependentProperty { get; private set; }
        public string DependentPropertyDisplayName { get; set; }

        public ContingentValidationAttribute(string dependentProperty)
        {
            DependentProperty = dependentProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMessage;
            }

            return string.Format(ErrorMessageString, name, DependentPropertyDisplayName ?? DependentProperty);
        }

        public override string DefaultErrorMessage => "{0} is invalid due to {1}.";

        public override bool IsValid(object value, object container)
        {
            return IsValid(value, GetDependentPropertyValue(container), container);
        }

        public abstract bool IsValid(object value, object dependentValue, object container);

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return base.GetClientValidationParameters()
                .Union(new[] { new KeyValuePair<string, object>("DependentProperty", DependentProperty) });
        }

        private object GetDependentPropertyValue(object container)
        {
            var currentType = container.GetType();
            var value = container;

            foreach (string propertyName in DependentProperty.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }
    }
}
