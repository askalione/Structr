using Structr.Email;

namespace Structr.Tests.Email.TestUtils
{
    internal class CustomEmailTemplateMessage : EmailTemplateMessage<CustomModel>
    {
        public override string Template => "Letter of {{From}} to {{To}}.";

        public CustomEmailTemplateMessage(string to, CustomModel model)
            : base(new EmailAddress(to), model)
        { }
    }
}
