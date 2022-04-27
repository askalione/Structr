using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Email
{
    public class EmailAddress
    {
        public string? Name { get; set; }
        public string Address { get; }

        public EmailAddress(string address, string? name)
            : this(address)
        {
            Name = name;
        }

        public EmailAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            Address = address;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name)
                ? Address
                : $"{Name} <{Address}>";
        }
    }
}
