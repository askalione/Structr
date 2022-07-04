using Structr.Domain;
using Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects;

namespace Structr.Samples.EntityFrameworkCore.WebApp.Domain.Issues
{
    public class Issue : Entity<Issue, int>
    {
        public int ProjectId { get; private set; }
        public virtual Project Project { get; private set; } = default!;

        public Multilang Description { get; private set; } = default!;

        private Issue() : base() { }

        public Issue(Project project, Multilang description)
        {
            ProjectId = project.Id;
            Project = project;
            Description = description;
        }
    }
}
