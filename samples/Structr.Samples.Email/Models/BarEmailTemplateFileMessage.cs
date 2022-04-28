using Structr.Email;

namespace Structr.Samples.Email.Models
{
    public class BarEmailTemplateFileMessage : EmailTemplateFileMessage<BarEmailModel>
    {
        public override string TemplatePath => "Bar.cshtml";

        public BarEmailTemplateFileMessage(string to, BarEmailModel model) : base(to, model)
        {
        }

        public BarEmailTemplateFileMessage(IEnumerable<string> to, BarEmailModel model) : base(to, model)
        {
        }

        public BarEmailTemplateFileMessage(IEnumerable<EmailAddress> to, BarEmailModel model) : base(to, model)
        {
        }
    }
}
