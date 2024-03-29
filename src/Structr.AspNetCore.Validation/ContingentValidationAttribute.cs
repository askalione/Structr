using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Serves as the base class for ASP.NET Core validation attributes.
    /// Contains basic conventions and functionality to work with property with which value the value of
    /// validating property will be compared.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ContingentValidationAttribute : ModelAwareValidationAttribute
    {
        /// <summary>
        /// Gets the related property with which value the value of validating property will be compared.
        /// </summary>
        public string RelatedProperty { get; private set; }

        /// <summary>
        /// Gets display name of the related property with which value the value of validating property will be compared.
        /// </summary>
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

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">Value of property to validate.</param>
        /// <param name="relatedPropertyValue">Value of related property to compare with.</param>
        /// <param name="container">Object containing this property.</param>
        /// <returns><see langword="true"/> if validation is successful; otherwise, <see langword="false"/>.</returns>
        public abstract bool IsValid(object value, object relatedPropertyValue, object container);

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

        protected static bool PropertyIsEmpty(object propertyValue)
        {
            if (propertyValue == null)
            {
                return true;
            }
            if (string.IsNullOrEmpty(propertyValue.ToString().Trim()))
            {
                return true;
            }
            if (propertyValue is IEnumerable valueList)
            {
                return valueList.OfType<object>().Any() == false;
            }
            return false;
        }
    }
}
