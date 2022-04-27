using RazorLight;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Structr.Email.Razor
{
    public class RazorEmailTemplateRenderer : IEmailTemplateRenderer
    {
        private readonly RazorLightEngine _engine;

        public RazorEmailTemplateRenderer(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(path)
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
