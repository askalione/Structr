using Structr.Email;

namespace Structr.Samples.Email.Models
{
    public class FooEmailTemplateFileMessage : EmailTemplateFileMessage<FooEmailModel>
    {
        public override string TemplatePath => "Foo.html";

        public FooEmailTemplateFileMessage(string to, FooEmailModel model) : base(to, model)
        {
        }

        public FooEmailTemplateFileMessage(IEnumerable<string> to, FooEmailModel model) : base(to, model)
        {
        }

        public FooEmailTemplateFileMessage(IEnumerable<EmailAddress> to, FooEmailModel model) : base(to, model)
        {
        }
    }
}
