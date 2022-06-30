using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Issues;

namespace Structr.Samples.EntityFrameworkCore.WebApp.DataAccess.Configurations.Issues
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.Description);

            builder.HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Issues");
        }
    }
}
