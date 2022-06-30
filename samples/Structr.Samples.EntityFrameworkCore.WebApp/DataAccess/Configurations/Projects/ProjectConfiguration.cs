using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects;

namespace Structr.Samples.EntityFrameworkCore.WebApp.DataAccess.Configurations.Projects
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Name);

            builder.ToTable("Projects");
        }
    }
}
