using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must match a value of specified related property.
    /// </summary>
    public class IsAttribute : ContingentValidationAttribute
    {
        /// <summary>
        /// Operator to be used in comparision during validation.
        /// </summary>
        public Operator Operator { get; private set; }

        /// <summary>
        /// If <see langword="true"/> then indicates that validation should be passed if value of property to
        /// be validated or value of related property equals <see langword="null"/>. In case of both values
        /// are <see langword="null"/> then behavior of validation will depend on type of <see cref="Validation.Operator"/>
        /// being used.
        /// </summary>
        public bool PassOnNull { get; set; }

        private OperatorMetadata _metadata;

        /// <summary>
        /// Checks value in respect to value of provided related property, using specified comparision operator.
        /// </summary>
        /// <param name="operator">Operator to be used in comparision during validation.</param>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
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

        public override bool IsValid(object value, object relatedPropertyValue, object container)
        {
            if (PassOnNull && (value == null || relatedPropertyValue == null) && (value != null || relatedPropertyValue != null))
            {
                return true;
            }

            return _metadata.IsValid(value, relatedPropertyValue);
        }

        public override string DefaultErrorMessage => "{0} must be " + _metadata.ErrorMessage + " {1}.";
    }
}
