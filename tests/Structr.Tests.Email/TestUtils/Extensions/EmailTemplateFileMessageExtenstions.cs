using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailTemplateFileMessageExtenstions
    {
        public static EmailTemplateFileMessageAssertions Should(this EmailTemplateFileMessage instance)
        {
            return new EmailTemplateFileMessageAssertions(instance);
        }
    }
}
