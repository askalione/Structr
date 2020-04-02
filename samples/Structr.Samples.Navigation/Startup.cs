using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Structr.Navigation;
using Structr.Samples.Navigation.Infrastructure;
using Structr.Samples.Navigation.Resources;

namespace Structr.Samples.Navigation
{
    public class Startup
    {
        public IWebHostEnvironment Env { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            Env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            var mvcBuilder = services.AddControllersWithViews();
#if DEBUG
            if (Env.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }
#endif
            services.AddNavigation(config =>
            {
                var rootPath = Env.ContentRootPath;

                // Menu
                config.AddXml(Path.Combine(rootPath, "menu.xml"), new NavigationItemOptions<MenuItem>
                {
                    Resource = typeof(MenuResource),
                    Activator = (item, serviceProvider) =>
                    {
                        var _activator = serviceProvider.GetService<IMenuActivator>();
                        return _activator.Activate(item);
                    }
                });
                // Breadcrumbs
                config.AddJson(Path.Combine(rootPath, "breadcrumbs.json"), new NavigationItemOptions<Breadcrumb>
                {
                    Activator = (breadcrumb, serviceProvider) =>
                    {
                        var _activator = serviceProvider.GetService<IBreadcrumbActivator>();
                        return _activator.Activate(breadcrumb);
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
