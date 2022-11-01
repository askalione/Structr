;

if (!AspNetCoreValidation)
    throw "You must load the aspnetcore.validation.js script before this.";

AspNetCoreValidation.registerValidators = function(jQuery) {
    if (!jQuery)
        throw "You must load jquery library before this.";

    jQuery.validator.addMethod("is", function(value, element, params) {
        var relatedproperty = AspNetCoreValidation.getId(element, params["relatedproperty"]);
        var operator = params["operator"];
        var passOnNull = params["passonnull"];
        var relatedpropertyvalue = document.getElementById(relatedproperty).value;

        if (AspNetCoreValidation.is(value, operator, relatedpropertyvalue, passOnNull))
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredif", function(value, element, params) {
        var relatedproperty = AspNetCoreValidation.getName(element, params["relatedproperty"]);
        var dependentTestValue = params["relatedpropertyvalue"];
        var operator = params["operator"];
        var pattern = params["pattern"];
        var relatedpropertyElement = document.getElementsByName(relatedproperty);
        var relatedpropertyvalue = null;

        if (relatedpropertyElement.length > 1) {
            for (var index = 0; index != relatedpropertyElement.length; index++)
                if (relatedpropertyElement[index]["checked"]) {
                    relatedpropertyvalue = relatedpropertyElement[index].value;
                    break;
                }

            if (relatedpropertyvalue == null)
                relatedpropertyvalue = false
        }
        else
            relatedpropertyvalue = relatedpropertyElement[0].value;

        if (AspNetCoreValidation.is(relatedpropertyvalue, operator, dependentTestValue)) {
            if (pattern == null) {
                if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                    return true;
            }
            else
                return (new RegExp(pattern)).test(value);
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifempty", function(value, element, params) {
        var relatedproperty = AspNetCoreValidation.getId(element, params["relatedproperty"]);
        var relatedpropertyvalue = document.getElementById(relatedproperty).value;

        if (relatedpropertyvalue == null || relatedpropertyvalue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') == "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });

    jQuery.validator.addMethod("requiredifnotempty", function(value, element, params) {
        var relatedproperty = AspNetCoreValidation.getId(element, params["relatedproperty"]);
        var relatedpropertyvalue = document.getElementById(relatedproperty).value;

        if (relatedpropertyvalue != null && relatedpropertyvalue.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "") {
            if (value != null && value.toString().replace(/^\s\s*/, '').replace(/\s\s*$/, '') != "")
                return true;
        }
        else
            return true;

        return false;
    });
};

(AspNetCoreValidation.registerValidators)(jQuery);