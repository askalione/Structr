using Structr.Email;

namespace Structr.Samples.Email.Models
{
    public class FooEmailTemplateFileMessage : EmailTemplateFileMessage<FooEmailModel>
    {
        public override string TemplatePath => "Foo.html";

        public FooEmailTemplateFileMessage(string to, FooEmailModel model)
            : base(new EmailAddress(to), model)
        { }
    }
}
