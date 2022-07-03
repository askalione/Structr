using RazorLight;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Email.Razor
{
    /// <summary>
    /// Provides functionality for rendering an email model into a template with Razor rendering.
    /// </summary>
    public class RazorEmailTemplateRenderer : IEmailTemplateRenderer // TODO: make internal
    {
        private readonly RazorLightEngine _engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="RazorEmailTemplateRenderer"/> class.
        /// </summary>
        /// <param name="options">The <see cref="EmailOptions"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">If email option "TemplateRootPath" not specified..</exception>
        /// <exception cref="DirectoryNotFoundException">If directory specified in email option "TemplateRootPath" not found.</exception>
        public RazorEmailTemplateRenderer(EmailOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (string.IsNullOrWhiteSpace(options.TemplateRootPath))
            {
                throw new InvalidOperationException($"Email option \"TemplateRootPath\" not specified.");
            }
            if (Directory.Exists(options.TemplateRootPath) == false)
            {
                throw new DirectoryNotFoundException($"Directory specified in email option \"TemplateRootPath\" not found.");
            }

            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(options.TemplateRootPath)
                .UseMemoryCachingProvider()
                .Build();
        }

        /// <remarks>
        /// Works with public model classes only.
        /// </remarks>
        /// <inheritdoc />
        public Task<string> RenderAsync<TModel>(string template, TModel model)
        {
            dynamic? viewBag = (model as IRazorModel)?.ViewBag;
            return _engine.CompileRenderStringAsync<TModel>(GetTemplateKey(template), template, model, viewBag);
        }

        private string GetTemplateKey(string template)
        {
            using (var algorithm = SHA256.Create())
            {
                byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(template));
                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                string result = sb.ToString();
                return result;
            }
        }
    }
}
