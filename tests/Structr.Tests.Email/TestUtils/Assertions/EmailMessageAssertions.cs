using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Structr.Email;

namespace Structr.Tests.Email.TestUtils.Assertions
{
    internal class EmailMessageAssertions : ReferenceTypeAssertions<EmailMessage, EmailMessageAssertions>
    {
        public EmailMessageAssertions(EmailMessage instance)
            : base(instance)
        { }

        protected override string Identifier => "email message";

        public AndConstraint<EmailMessageAssertions> HaveMessageAndReceiver(
            string message, string to, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .Given(() => Subject)
                   .ForCondition(m => m.Message == message)
                   .FailWith("Expected {context:EmailMessage} to contain message with text {0}{reason}, but found {1}.", _ => message, x => x.Message)
                   .Then
                   .ForCondition(m => m.To.Address == to)
                   .FailWith("Expected {context:EmailMessage} to have receiver (To.Address) {0}{reason}, but found {1}.", _ => to, x => x.To.Address);

            return new AndConstraint<EmailMessageAssertions>(this);
        }
    }
}
