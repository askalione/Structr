using RazorLight;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Email.Razor
{
    public class RazorEmailTemplateRenderer : IEmailTemplateRenderer
    {
        private readonly RazorLightEngine _engine;

        public RazorEmailTemplateRenderer(EmailOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (string.IsNullOrEmpty(options.TemplateRootPath))
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

        public Task<string> RenderAsync<TModel>(string template, TModel model)
        {
            dynamic viewBag = (model as IRazorModel)?.ViewBag;
            return _engine.CompileRenderStringAsync<TModel>(GetTemplateKey(template), template, model, viewBag);
        }

        private string GetTemplateKey(string template)
        {
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(template));
                var sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
