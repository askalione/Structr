using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Marks property as required when specified property matches specified condition.
    /// </summary>
    public class RequiredIfAttribute : ContingentValidationAttribute
    {
        /// <summary>
        /// Gets comparision operator.
        /// </summary>
        public Operator Operator { get; private set; }

        /// <summary>
        /// Gets expected value of related property to compare with.
        /// </summary>
        public object RelatedPropertyExpectedValue { get; private set; }

        /// <summary>
        /// Contains an instance of <see cref="OperatorMetadata"/> for specified <see cref="Validation.Operator"/>.
        /// </summary>
        protected OperatorMetadata Metadata { get; private set; }

        /// <summary>
        /// Marks property as required when condition for specified <paramref name="relatedProperty"/> is met.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        /// <param name="operator">Comparison operator to check related property's value.</param>
        /// <param name="relatedPropertyExpectedValue">Expected related property's value to compare with.</param>
        public RequiredIfAttribute(string relatedProperty, Operator @operator, object relatedPropertyExpectedValue)
            : base(relatedProperty)
        {
            Operator = @operator;
            RelatedPropertyExpectedValue = relatedPropertyExpectedValue;
            Metadata = OperatorMetadata.Get(Operator);
        }

        public RequiredIfAttribute(string relatedProperty, object relatedPropertyExpectedValue)
            : this(relatedProperty, Operator.EqualTo, relatedPropertyExpectedValue) { }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessageResourceName) && string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = DefaultErrorMessage;
            }
            return string.Format(ErrorMessageString, name, RelatedPropertyDisplayName ?? RelatedProperty, RelatedPropertyExpectedValue);
        }

        public override string ClientTypeName => "RequiredIf";

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return base.GetClientValidationParameters()
                .Union(new[] {
                    new KeyValuePair<string, object>("Operator", Operator.ToString()),
                    new KeyValuePair<string, object>("RelatedPropertyValue", RelatedPropertyExpectedValue)
                });
        }

        public override bool IsValid(object value, object relatedPropertyValue, object container)
        {
            if (Metadata.IsValid(relatedPropertyValue, RelatedPropertyExpectedValue))
            {
                return PropertyIsEmpty(value) == false;
            }
            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} being " + Metadata.ErrorMessage + " {2}.";
    }
}
