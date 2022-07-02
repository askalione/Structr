using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Structr.Tests.Email.TestUtils.Assertions
{
    internal class CustomEmailTemplateFileMessageAssertions : ReferenceTypeAssertions<CustomEmailTemplateFileMessage, CustomEmailTemplateFileMessageAssertions>
    {
        public CustomEmailTemplateFileMessageAssertions(CustomEmailTemplateFileMessage instance)
            : base(instance)
        { }

        protected override string Identifier => "custom email template message";

        public AndConstraint<CustomEmailTemplateFileMessageAssertions> HaveTemplatePathReceiverAndModel(
            string templatePath, string to, object model, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject)
                   .ForCondition(m => m.TemplatePath == templatePath)
                   .FailWith("Expected {context:CustomEmailTemplateFileMessage} to have template path {0}{reason}, but found {1}.", _ => templatePath, x => x.TemplatePath)
                   .Then
                   .ForCondition(m => m.To.Address == to)
                   .FailWith("Expected {context:CustomEmailTemplateFileMessage} to have receiver (To.Address) {0}{reason}, but found {1}.", _ => to, x => x.To.Address)
                   .Then
                   .ForCondition(m => m.Model == model)
                   .FailWith("Expected {context:CustomEmailTemplateFileMessage} to contain provided model{reason}, but found another one.");

            return new AndConstraint<CustomEmailTemplateFileMessageAssertions>(this);
        }
    }
}
