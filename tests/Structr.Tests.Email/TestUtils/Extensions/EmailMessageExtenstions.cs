using Structr.Email;
using Structr.Tests.Email.TestUtils.Assertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Tests.Email.TestUtils.Extensions
{
    internal static class EmailMessageExtenstions
    {
        public static EmailMessageAssertions Should(this EmailMessage instance)
        {
            return new EmailMessageAssertions(instance);
        }
    }
}
