using Structr.Domain;

namespace Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects
{
    public class Project : Entity<Project, int>
    {
        public string Code { get; private set; } = default!;
        public Multilang Name { get; private set; } = default!;

        private Project() : base() { }

        public Project(string code, Multilang name)
        {
            Code = code;
            Name = name;
        }
    }
}
