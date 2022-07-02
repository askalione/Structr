using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateMessageExtenstions
    {
        public static EmailTemplateMessageAssertions Should(this EmailTemplateMessage instance)
        {
            return new EmailTemplateMessageAssertions(instance);
        }
    }
}
