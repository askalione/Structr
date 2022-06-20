using Structr.Email;

namespace Structr.Tests.Email.TestUtils
{
    internal class CustomEmailTemplateFileMessage : EmailTemplateFileMessage<CustomModel>
    {
        public CustomEmailTemplateFileMessage(string to, CustomModel model) : base(to, model) { }
        public CustomEmailTemplateFileMessage(IEnumerable<string> to, CustomModel model) : base(to, model) { }
        public CustomEmailTemplateFileMessage(IEnumerable<EmailAddress> to, CustomModel model) : base(to, model) { }

        public override string TemplatePath => TestDataPath.ContentRootPath;
    }
}
