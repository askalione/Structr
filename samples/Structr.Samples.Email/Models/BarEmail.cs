using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Samples.Email.Models
{
    public record BarEmail
    {
        public string Message { get; init; } = default!;
    }
}
