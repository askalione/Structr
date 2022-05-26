using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ContingentValidationAttribute : ModelAwareValidationAttribute
    {
        public string RelatedProperty { get; private set; }
        public string RelatedPropertyDisplayName { get; set; }

        public ContingentValidationAttribute(string relatedProperty)
        {
            RelatedProperty = relatedProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMessage;
            }

            return string.Format(ErrorMessageString, name, RelatedPropertyDisplayName ?? RelatedProperty);
        }

        public override string DefaultErrorMessage => "{0} is invalid due to {1}.";

        public override bool IsValid(object value, object container)
        {
            return IsValid(value, GetRelatedPropertyValue(container), container);
        }

        public abstract bool IsValid(object value, object relatedValue, object container);

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return base.GetClientValidationParameters()
                .Union(new[] { new KeyValuePair<string, object>("RelatedProperty", RelatedProperty) });
        }

        private object GetRelatedPropertyValue(object container)
        {
            var currentType = container.GetType();
            var value = container;

            foreach (string propertyName in RelatedProperty.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }
    }
}
