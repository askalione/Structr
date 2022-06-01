#nullable disable

using Structr.AspNetCore.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace Structr.Tests.AspNetCore.Validation.TestUtils
{
    internal static class TestValidation
    {
        private class TestModel
        {
            public object Value1 { get; set; }
            public object Value2 { get; set; }
        }

        public static ValidationResult TestIs<TAttribute>(object value1,
            object value2,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null,
            bool? passNull = null)
            where TAttribute : IsAttribute
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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

        public static ValidationResult TestRequiredIfEmpty(object value1,
            object value2,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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

        public static ValidationResult TestRequiredIfNotEmpty(object value1,
            object value2,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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

        public static ValidationResult TestRequiredIf<TAttribute>(object value1,
            object value2,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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

        public static ValidationResult TestRequiredIf<TAttribute>(object value1,
            object value2,
            object relatedPropertyExpectedValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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

        public static ValidationResult TestRequiredIfRegEx<TAttribute>(object value1,
            object value2,
            string pattern,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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

        public static ValidationResult TestRegularExpressionIf(object value1,
            object value2,
            object relatedPropertyExpectedValue,
            string pattern,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

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