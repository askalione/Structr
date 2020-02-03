namespace Structr.EntityFrameworkCore.Options
{
    public class AuditableConfigurationOptions
    {
        public int SignedColumnMaxLength { get; set; }
        public bool SignedColumnIsRequired { get; set; }

        public AuditableConfigurationOptions()
        {
            SignedColumnMaxLength = 50;
            SignedColumnIsRequired = false;
        }
    }
}
