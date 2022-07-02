using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Assertions
{
    internal class EmailTemplateFileMessageAssertions : ReferenceTypeAssertions<EmailTemplateFileMessage, EmailTemplateFileMessageAssertions>
    {
        public EmailTemplateFileMessageAssertions(EmailTemplateFileMessage instance)
            : base(instance)
        { }

        protected override string Identifier => "email template message";

        public AndConstraint<EmailTemplateFileMessageAssertions> HaveTemplatePathReceiverAndModel(
            string templatePath, string to, object model, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject)
                   .ForCondition(m => m.TemplatePath == templatePath)
                   .FailWith("Expected {context:EmailTemplateFileMessage} have template path {0}{reason}, but found {1}.", _ => templatePath, x => x.TemplatePath)
                   .Then
                   .ForCondition(m => m.To.Address == to)
                   .FailWith("Expected {context:EmailTemplateFileMessage} to have receiver (To.Address) {0}{reason}, but found {1}.", _ => to, x => x.To.Address)
                   .Then
                   .ForCondition(m => m.Model == model)
                   .FailWith("Expected {context:EmailTemplateFileMessage} to contain provided model{reason}, but found another one.");

            return new AndConstraint<EmailTemplateFileMessageAssertions>(this);
        }
    }
}
