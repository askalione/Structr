using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    public class IsAttribute : ContingentValidationAttribute
    {
        public Operator Operator { get; private set; }
        public bool PassOnNull { get; set; }

        private OperatorMetadata _metadata;

        public IsAttribute(Operator @operator, string relatedProperty)
            : base(relatedProperty)
        {
            Operator = @operator;
            PassOnNull = false;
            _metadata = OperatorMetadata.Get(Operator);
        }

        public override string ClientTypeName => "Is";

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return base.GetClientValidationParameters()
                .Union(new[]
                       {
                           new KeyValuePair<string, object>("Operator", Operator.ToString()),
                           new KeyValuePair<string, object>("PassOnNull", PassOnNull)
                       });
        }

        public override bool IsValid(object value, object relatedValue, object container)
        {
            if (PassOnNull && (value == null || relatedValue == null) && (value != null || relatedValue != null))
            {
                return true;
            }

            return _metadata.IsValid(value, relatedValue);
        }

        public override string DefaultErrorMessage => "{0} must be " + _metadata.ErrorMessage + " {1}.";
    }
}
