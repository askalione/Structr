using System;

namespace Structr.Operations.Internal
{
    internal static class HandlerProvider
    {
        public static THandler GetHandler<THandler>(IServiceProvider serviceProvider)
        {
            var handlerType = typeof(THandler);
            THandler handler;

            try
            {
                handler = (THandler)serviceProvider.GetService(handlerType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error constructing operation handler of type {handlerType}", ex);
            }

            if (handler == null)
                throw new InvalidOperationException($"Operation handler of type {handlerType} was not found");

            return handler;
        }
    }
}
