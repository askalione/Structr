using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Email
{
    internal class Test
    {
        public class HelloUserEmailModel
        {
            public string Name { get; set; } = default!;
        }

        public class HelloUserEmailTemplate : EmailTemplate<HelloUserEmailModel>
        {
            public override string Template => "Hello, @Model.Name!";

            public HelloUserEmailTemplate(string to, HelloUserEmailModel model) : base(to, model)
            {
            }

            public HelloUserEmailTemplate(IEnumerable<string> to, HelloUserEmailModel model) : base(to, model)
            {
            }

            public HelloUserEmailTemplate(IEnumerable<EmailAddress> to, HelloUserEmailModel model) : base(to, model)
            {
            }
        }

        public void TestMethod()
        {
            var emailMessage = new EmailMessage(new[] { "test@example.com" }, "Hello world!");
            var emailTemplate = new EmailTemplate("test@example.com", "Hello, @Model.Name!", new { Name = "Alexey" });
            var emailTemplate2 = new HelloUserEmailTemplate("test@example.com", new HelloUserEmailModel
            {
                Name = "Alexey"
            });
        }
    }
}
