using Structr.Email;

namespace Structr.Samples.Email.Models
{
    public class FooEmailTemplateFile : EmailTemplateFile<FooEmail>
    {
        public override string TemplatePath => "Foo.html";

        public FooEmailTemplateFile(string to, FooEmail model) : base(to, model)
        {
        }

        public FooEmailTemplateFile(IEnumerable<string> to, FooEmail model) : base(to, model)
        {
        }

        public FooEmailTemplateFile(IEnumerable<EmailAddress> to, FooEmail model) : base(to, model)
        {
        }
    }
}
