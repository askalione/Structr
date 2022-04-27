using Structr.Email;

namespace Structr.Samples.Email.Models
{
    public class BarEmailTemplateFile : EmailTemplateFile<BarEmail>
    {
        public override string TemplatePath => "Bar.cshtml";

        public BarEmailTemplateFile(string to, BarEmail model) : base(to, model)
        {
        }

        public BarEmailTemplateFile(IEnumerable<string> to, BarEmail model) : base(to, model)
        {
        }

        public BarEmailTemplateFile(IEnumerable<EmailAddress> to, BarEmail model) : base(to, model)
        {
        }
    }
}
