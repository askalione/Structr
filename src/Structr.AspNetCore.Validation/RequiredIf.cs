using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    public class RequiredIfAttribute : ContingentValidationAttribute
    {
        public Operator Operator { get; private set; }
        public object RelatedPropertyExpectedValue { get; private set; }
        protected OperatorMetadata Metadata { get; private set; }

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
                return value != null && string.IsNullOrEmpty(value.ToString().Trim()) == false;
            }
            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} being " + Metadata.ErrorMessage + " {2}.";
    }
}
