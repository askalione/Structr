using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class CustomEmailTemplateFileMessageExtenstions
    {
        public static CustomEmailTemplateFileMessageAssertions Should(this CustomEmailTemplateFileMessage instance)
        {
            return new CustomEmailTemplateFileMessageAssertions(instance);
        }
    }
}
