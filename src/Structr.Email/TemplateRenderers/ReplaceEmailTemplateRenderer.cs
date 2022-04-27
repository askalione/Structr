using System.Reflection;
using System.Threading.Tasks;

namespace Structr.Email.TemplateRenderers
{
    public class ReplaceEmailTemplateRenderer : IEmailTemplateRenderer
    {
        public Task<string> RenderAsync<TModel>(string template, TModel model)
        {
            foreach (var pi in model!.GetType().GetRuntimeProperties())
            {
                template = template.Replace($"{{{{{pi.Name}}}}}", pi.GetValue(model, null).ToString());
            }

            return Task.FromResult(template);
        }
    }
}
