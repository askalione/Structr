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

        public static ValidationResult TestIs<TAttribute>(object value1,
            object value2,
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null,
            bool? passNull = null)
            where TAttribute : IsAttribute
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

            var attribute = Activator.CreateInstance(typeof(TAttribute), nameof(TestModel.Value2)) as TAttribute;

            if (dependentPropertyDisplayName != null)
            {
                attribute.DependentPropertyDisplayName = dependentPropertyDisplayName;
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
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

            var attribute = new RequiredIfEmptyAttribute(nameof(TestModel.Value2));

            if (dependentPropertyDisplayName != null)
            {
                attribute.DependentPropertyDisplayName = dependentPropertyDisplayName;
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
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

            var attribute = new RequiredIfNotEmptyAttribute(nameof(TestModel.Value2));

            if (dependentPropertyDisplayName != null)
            {
                attribute.DependentPropertyDisplayName = dependentPropertyDisplayName;
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
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
            where TAttribute : RequiredIfAttribute
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

            var attribute = Activator.CreateInstance(typeof(TAttribute), nameof(TestModel.Value2)) as TAttribute;

            if (dependentPropertyDisplayName != null)
            {
                attribute.DependentPropertyDisplayName = dependentPropertyDisplayName;
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

        public static ValidationResult TestRequiredIfNot(object value1,
            object value2,
            object notValue2,
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

            var attribute = new RequiredIfNotAttribute(nameof(TestModel.Value2), notValue2);

            if (dependentPropertyDisplayName != null)
            {
                attribute.DependentPropertyDisplayName = dependentPropertyDisplayName;
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