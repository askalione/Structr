#nullable disable

using Structr.AspNetCore.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.AspNetCore.Validation.TestUtils
{
    internal static class TestValidation
    {
        private class TestModel
        {
            public object Value1 { get; set; }
            public object Value2 { get; set; }
        }

        public static ValidationResult TestIs<TAttribute>(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null,
            bool? passNull = null)
            where TAttribute : IsAttribute
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = Activator.CreateInstance(typeof(TAttribute), nameof(TestModel.Value2)) as TAttribute;

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }
            if (passNull != null)
            {
                attribute.PassOnNull = (bool)passNull;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }

        public static ValidationResult TestRequiredIfEmpty(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = new RequiredIfEmptyAttribute(nameof(TestModel.Value2));

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }

        public static ValidationResult TestRequiredIfNotEmpty(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = new RequiredIfNotEmptyAttribute(nameof(TestModel.Value2));

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }

        public static ValidationResult TestRequiredIf<TAttribute>(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = Activator.CreateInstance(typeof(TAttribute), nameof(TestModel.Value2)) as TAttribute;

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }

        public static ValidationResult TestRequiredIf<TAttribute>(object propertyValue,
            object relatedPropertyValue,
            object relatedPropertyExpectedValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = Activator.CreateInstance(typeof(TAttribute), nameof(TestModel.Value2), relatedPropertyExpectedValue) as TAttribute;

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }

        public static ValidationResult TestRequiredIfRegEx<TAttribute>(object propertyValue,
            object relatedPropertyValue,
            string pattern,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = Activator.CreateInstance(typeof(TAttribute), nameof(TestModel.Value2), pattern) as TAttribute;

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }

        public static ValidationResult TestRegularExpressionIf(object propertyValue,
            object relatedPropertyValue,
            object relatedPropertyExpectedValue,
            string pattern,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = propertyValue, Value2 = relatedPropertyValue };

            var attribute = new RegularExpressionIfAttribute(pattern, nameof(TestModel.Value2), relatedPropertyExpectedValue);

            if (relatedPropertyDisplayName != null)
            {
                attribute.RelatedPropertyDisplayName = relatedPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }
    }
}