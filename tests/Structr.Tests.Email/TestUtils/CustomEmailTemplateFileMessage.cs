using Structr.Email;

namespace Structr.Tests.Email.TestUtils
{
    internal class CustomEmailTemplateFileMessage : EmailTemplateFileMessage<CustomModel>
    {
        public override string TemplatePath => TestDataPath.ContentRootPath;

        public CustomEmailTemplateFileMessage(string to, CustomModel model)
            : base(new EmailAddress(to), model)
        { }
    }
}
