using Microsoft.Extensions.DependencyInjection;
using Structr.AspNetCore.Client.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="Controller"/>.
    /// </summary>
    public static class ClientOptionControllerExtensions
    {
        /// <summary>
        /// Adds client options to context associated with controller.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="options">Options to add represented by object.</param>
        public static void AddClientOptions(this Controller controller, object options)
        {
            var key = GetClientOptionKey(controller);
            AddClientOptions(controller, key, options);
        }

        /// <param name="key">Key to use for storing options.</param>
        /// <inheritdoc cref="AddClientOptions(Controller, object)"/>
        public static void AddClientOptions(this Controller controller, string key, object options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var optionsType = options.GetType();
            var optionsProps = optionsType.GetProperties();
            var optionsAsDictionary = optionsProps.ToDictionary(x => x.Name, x => x.GetValue(options, null));
            AddClientOptions(controller, key, optionsAsDictionary);
        }

        /// <param name="options">Options to add represented by dictionary.</param>
        /// <inheritdoc cref="AddClientOptions(Controller, object)"/>
        public static void AddClientOptions(this Controller controller, Dictionary<string, object> options)
        {
            var key = GetClientOptionKey(controller);
            AddClientOptions(controller, key, options);
        }

        /// <inheritdoc cref="AddClientOptions(Controller, Dictionary{string, object})"/>
        /// <inheritdoc cref="AddClientOptions(Controller, string, object)"/>
        public static void AddClientOptions(this Controller controller, string key, Dictionary<string, object> options)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            var optionProvider = GetClientOptionProvider(controller);
            optionProvider.AddClientOptions(key, options);
        }

        private static string GetClientOptionKey(Controller controller)
            => GetClientOptionProvider(controller).BuildClientOptionsKey(controller.RouteData);

        private static IClientOptionProvider GetClientOptionProvider(Controller controller)
            => controller.HttpContext.RequestServices
                .GetRequiredService<IClientOptionProvider>();
    }
}
