using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Collections.Infrastructure
{
    public abstract class Writable
    {
        public override string ToString()
        {
            string result = "";
            foreach (var prop in GetType().GetProperties())
                result += $"{prop.Name}={prop.GetValue(this, null)};";
            return result;
        }
    }
}
