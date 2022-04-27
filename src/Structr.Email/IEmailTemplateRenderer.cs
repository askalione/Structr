using System.Threading.Tasks;

namespace Structr.Email
{
    public interface IEmailTemplateRenderer
    {
        Task<string> RenderAsync<TModel>(string template, TModel model);
    }
}
