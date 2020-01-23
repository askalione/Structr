using Structr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structr.EntityFramework.Options
{
    public class AuditableConfigurationOptions
    {
        public int SignedColumnMaxLength { get; set; }
        public bool SignedColumnIsRequired { get; set; }
        public string SoftDeletableFilterName { get; set; }

        public AuditableConfigurationOptions()
        {
            SignedColumnMaxLength = 50;
            SignedColumnIsRequired = false;
            SoftDeletableFilterName = "SoftDeletable";
        }
    }
}
