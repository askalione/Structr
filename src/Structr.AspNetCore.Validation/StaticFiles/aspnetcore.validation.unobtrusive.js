;

if (!AspNetCoreValidation)
    throw "You must load the aspnetcore.validation.js script before this.";

if (!AspNetCoreValidation.registerValidators)
    throw "You must load the aspnetcore.jquery.validation.js script before this.";

var setValidationValues = function(options, ruleName, value) {
    options.rules[ruleName] = value;
    if (options.message) {
        options.messages[ruleName] = options.message;
    }
};

var $Unob = $.validator.unobtrusive;

$Unob.adapters.add("requiredif", ["relatedproperty", "relatedpropertyvalue", "operator", "pattern"], function(options) {
    var value = {
        relatedproperty: options.params.relatedproperty,
        relatedpropertyvalue: options.params.relatedpropertyvalue,
        operator: options.params.operator,
        pattern: options.params.pattern
    };
    setValidationValues(options, "requiredif", value);
});

$Unob.adapters.add("is", ["relatedproperty", "operator", "passonnull"], function(options) {
    setValidationValues(options, "is", {
        relatedproperty: options.params.relatedproperty,
        operator: options.params.operator,
        passonnull: options.params.passonnull
    });
});

$Unob.adapters.add("requiredifempty", ["relatedproperty"], function(options) {
    setValidationValues(options, "requiredifempty", {
        relatedproperty: options.params.relatedproperty
    });
});

$Unob.adapters.add("requiredifnotempty", ["relatedproperty"], function(options) {
    setValidationValues(options, "requiredifnotempty", {
        relatedproperty: options.params.relatedproperty
    });
});