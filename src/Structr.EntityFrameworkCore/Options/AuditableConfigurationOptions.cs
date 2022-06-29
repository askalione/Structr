namespace Structr.EntityFrameworkCore.Options
{
    /// <summary>
    /// Defines a set of options for configuring Auditable Entities.
    /// </summary>
    public class AuditableConfigurationOptions
    {
        /// <summary>
        /// Defines the maximum size of a signed column (CreatedBy, ModifiedBy, DeletedBy).
        /// </summary>
        public int SignedColumnMaxLength { get; set; }

        /// <summary>
        /// Defines if a signed column (CreatedBy, ModifiedBy, DeletedBy) is required.
        /// </summary>
        public bool SignedColumnIsRequired { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="AuditableConfigurationOptions"/> with default values.
        /// </summary>
        public AuditableConfigurationOptions()
        {
            SignedColumnMaxLength = 50;
            SignedColumnIsRequired = false;
        }
    }
}
