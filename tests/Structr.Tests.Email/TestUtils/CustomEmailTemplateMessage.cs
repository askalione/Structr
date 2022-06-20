using Structr.Email;

namespace Structr.Tests.Email.TestUtils
{
    internal class CustomEmailTemplateMessage : EmailTemplateMessage<CustomModel>
    {
        public CustomEmailTemplateMessage(string to, CustomModel model) : base(to, model) { }
        public CustomEmailTemplateMessage(IEnumerable<string> to, CustomModel model) : base(to, model) { }
        public CustomEmailTemplateMessage(IEnumerable<EmailAddress> to, CustomModel model) : base(to, model) { }

        public override string Template => "Letter of {{From}} to {{To}}.";
    }
}
