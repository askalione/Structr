using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Serves as the base class for all <see cref="Structr"/>.<see cref="AspNetCore"/> validation attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ModelAwareValidationAttribute : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMessage;
            }

            return base.FormatErrorMessage(name);
        }

        /// <summary>
        /// Default error message to be shown when validation fails.
        /// </summary>
        public virtual string DefaultErrorMessage => "{0} is invalid.";

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">Value of property to validate.</param>
        /// <param name="container">Object containing this property.</param>
        /// <returns><see langword="true"/> if validation is successful; otherwise, <see langword="false"/>.</returns>
        public abstract bool IsValid(object value, object container);

        /// <summary>
        /// Returns attribute name.
        /// </summary>
        public virtual string ClientTypeName => this.GetType().Name.Replace("Attribute", "");

        /// <summary>
        /// Returns validation parameters list if form of dictionary, conatining name of parameter and its value.
        /// </summary>
        public Dictionary<string, object> ClientValidationParameters =>
            GetClientValidationParameters().ToDictionary(kv => kv.Key.ToLower(), kv => kv.Value);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool validate = IsValid(value, validationContext.ObjectInstance);
            if (validate)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
            }
        }

        /// <summary>
        /// Returns a list of validation parameters in form of dictionary.
        /// </summary>
        protected virtual IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return new KeyValuePair<string, object>[0];
        }
    }
}
