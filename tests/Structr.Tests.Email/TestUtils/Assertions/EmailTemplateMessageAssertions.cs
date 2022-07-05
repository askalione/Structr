using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Assertions
{
    internal class EmailTemplateMessageAssertions : ReferenceTypeAssertions<EmailTemplateMessage, EmailTemplateMessageAssertions>
    {
        public EmailTemplateMessageAssertions(EmailTemplateMessage instance)
            : base(instance)
        { }

        protected override string Identifier => "email template message";

        public AndConstraint<EmailTemplateMessageAssertions> HaveTemplateReceiverAndModel(
            string template, string to, object model, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject)
                   .ForCondition(m => m.Template == template)
                   .FailWith("Expected {context:EmailTemplateMessage} to contain template {0}{reason}, but found {1}.", _ => template, x => x.Template)
                   .Then
                   .ForCondition(m => m.To.Address == to)
                   .FailWith("Expected {context:EmailTemplateMessage} to have receiver (To.Address) {0}{reason}, but found {1}.", _ => to, x => x.To.Address)
                   .Then
                   .ForCondition(m => m.Model == model)
                   .FailWith("Expected {context:EmailTemplateMessage} to contain provided model{reason}, but found another one.");

            return new AndConstraint<EmailTemplateMessageAssertions>(this);
        }
    }
}
