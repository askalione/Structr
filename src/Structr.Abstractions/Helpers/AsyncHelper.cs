using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Abstractions.Helpers
{
    /// <summary>
    /// Provides tools to run asynchronious methods in non-async methods.
    /// </summary>
    public static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        /// <summary>
        /// Synchroniously executes provided async method and returns result of type <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of async metod result.</typeparam>
        /// <param name="func">Async method to execute.</param>
        /// <returns>Async method execution result.</returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            return _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Synchroniously executes provided async method.
        /// </summary>
        /// <typeparam name="TResult">Type of async metod result.</typeparam>
        /// <param name="func">Async method to execute.</param>
        public static void RunSync(Func<Task> func)
        {
            var cultureUi = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CurrentCulture;
            var principal = Thread.CurrentPrincipal;
            _myTaskFactory.StartNew(() =>
            {
                Thread.CurrentPrincipal = principal;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }
    }
}
