using System.Threading.Tasks;

namespace Structr.Email
{
    /// <summary>
    /// Provides functionality for rendering an email model into a template.
    /// </summary>
    public interface IEmailTemplateRenderer
    {
        /// <summary>
        /// Renders the email model into the template.
        /// </summary>
        /// <typeparam name="TModel">Type of model.</typeparam>
        /// <param name="template">The template content.</param>
        /// <param name="model">The email model.</param>
        /// <returns>Rendered email text.</returns>
        Task<string> RenderAsync<TModel>(string template, TModel model);
    }
}
