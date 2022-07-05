using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Structr.Tests.Email.TestUtils.Assertions
{
    internal class CustomEmailTemplateMessageAssertions : ReferenceTypeAssertions<CustomEmailTemplateMessage, CustomEmailTemplateMessageAssertions>
    {
        public CustomEmailTemplateMessageAssertions(CustomEmailTemplateMessage instance)
            : base(instance)
        { }

        protected override string Identifier => "custom email template message";

        public AndConstraint<CustomEmailTemplateMessageAssertions> HaveTemplateReceiverAndModel(
            string template, string to, object model, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject)
                   .ForCondition(m => m.Template == template)
                   .FailWith("Expected {context:CustomEmailTemplateMessage} to contain template {0}{reason}, but found {1}.", _ => template, x => x.Template)
                   .Then
                   .ForCondition(m => m.To.Address == to)
                   .FailWith("Expected {context:CustomEmailTemplateMessage} to have receiver (To.Address) {0}{reason}, but found {1}.", _ => to, x => x.To.Address)
                   .Then
                   .ForCondition(m => m.Model == model)
                   .FailWith("Expected {context:CustomEmailTemplateMessage} to contain provided model{reason}, but found another one.");

            return new AndConstraint<CustomEmailTemplateMessageAssertions>(this);
        }
    }
}
