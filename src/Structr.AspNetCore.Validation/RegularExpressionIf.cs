using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must match the specified regular expression when provided property has specified value.
    /// </summary>
    public class RegularExpressionIfAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Gets the regular expression pattern.
        /// </summary>
        /// <returns>The pattern to match.</returns>
        public string Pattern { get; set; }

        /// <summary>
        /// Specifies that a data field value must match the specified regular expression when provided property has specified value.
        /// </summary>
        /// <param name="pattern">The regular expression that is used to validate the data field value.</param>
        /// <param name="relatedProperty">Related property name.</param>
        /// <param name="operator">Type of checking operator to apply.</param>
        /// <param name="relatedPropertyExpectedValue">Expected value of related property.</param>
        public RegularExpressionIfAttribute(string pattern, string relatedProperty, Operator @operator, object relatedPropertyExpectedValue)
            : base(relatedProperty, @operator, relatedPropertyExpectedValue)
        {
            Pattern = pattern;
        }

        public RegularExpressionIfAttribute(string pattern, string relatedProperty, object relatedPropertyExpectedValue)
            : this(pattern, relatedProperty, Operator.EqualTo, relatedPropertyExpectedValue) { }

        /// <summary>
        /// When value of related property equals specified <paramref name="relatedPropertyValue"/> then checks whether the <paramref name="value"/> entered by the user matches the regular expression pattern.
        /// </summary>
        /// <param name="value">Value of current property.</param>
        /// <param name="relatedPropertyValue">Current value of related property.</param>
        /// <param name="container">Object containing both properties.</param>
        /// <returns><see langword="true"/> if validation is successful; otherwise, <see langword="false"/>.</returns>
        public override bool IsValid(object value, object relatedPropertyValue, object container)
        {
            if (Metadata.IsValid(relatedPropertyValue, RelatedPropertyExpectedValue))
            {
                return OperatorMetadata.Get(Operator.RegExMatch).IsValid(value, Pattern);
            }

            return true;
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return base.GetClientValidationParameters()
                .Union(new[] {
                    new KeyValuePair<string, object>("Pattern", Pattern),
                });
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMessage;
            }

            return string.Format(ErrorMessageString, name, RelatedPropertyDisplayName ?? RelatedProperty, RelatedPropertyExpectedValue, Pattern);
        }

        public override string DefaultErrorMessage =>
            "{0} must be in the format of {3} due to {1} being " + Metadata.ErrorMessage + " {2}";
    }
}
