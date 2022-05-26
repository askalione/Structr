using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value in ASP.NET Dynamic Data must match the specified regular
    /// expression when provided property has specified value.
    /// </summary>
    public class RegularExpressionIfAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Gets the regular expression pattern.
        /// </summary>
        /// <returns>The pattern to match.</returns>
        public string Pattern { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegularExpressionIfAttribute"/> class.
        /// </summary>
        /// <param name="pattern">The regular expression that is used to validate the data field value.</param>
        /// <param name="relatedProperty">Property</param>
        /// <param name="operator"></param>
        /// <param name="relatedValue"></param>
        public RegularExpressionIfAttribute(string pattern, string relatedProperty, Operator @operator, object relatedValue)
            : base(relatedProperty, @operator, relatedValue)
        {
            Pattern = pattern;
        }

        public RegularExpressionIfAttribute(string pattern, string relatedProperty, object relatedValue)
            : this(pattern, relatedProperty, Operator.EqualTo, relatedValue) { }

        public override bool IsValid(object value, object relatedValue, object container)
        {
            if (Metadata.IsValid(relatedValue, RelatedValue))
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

            return string.Format(ErrorMessageString, name, RelatedPropertyDisplayName ?? RelatedProperty, RelatedValue, Pattern);
        }

        public override string DefaultErrorMessage =>
            "{0} must be in the format of {3} due to {1} being " + Metadata.ErrorMessage + " {2}";
    }
}
