using Structr.Email;

namespace Structr.Samples.Email.Models
{
    public class BarEmailTemplateFileMessage : EmailTemplateFileMessage<BarEmailModel>
    {
        public override string TemplatePath => "Bar.cshtml";

        public BarEmailTemplateFileMessage(string to, BarEmailModel model)
            : base(new EmailAddress(to), model)
        { }
    }
}
