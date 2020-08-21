using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Structr.AspNetCore.Validation.Internal
{
    internal class InternalValidationAttributeAdapterProvider : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
    {
        public InternalValidationAttributeAdapterProvider() { }

        IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
            ValidationAttribute attribute,
            IStringLocalizer stringLocalizer)
        {
            IAttributeAdapter adapter;
            if (attribute is ModelAwareValidationAttribute modelAwareValidationAttribute)
                adapter = new InternalValidationAttributeAdapter(modelAwareValidationAttribute, stringLocalizer);
            else
                adapter = GetAttributeAdapter(attribute, stringLocalizer);

            return adapter;
        }
    }
}
