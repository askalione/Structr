using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Structr.AspNetCore.Json
{
    /// <summary>
    /// Defines extension methods on <see cref="Controller"/>.
    /// </summary>
    public static class JsonControllerExtensions
    {
        /// <summary>
        /// Creates a <see cref="JsonResult"/> object, with serialized success marker.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="ok">Success marker.</param>
        /// <returns>An instance of <see cref="JsonResult"/>.</returns>
        public static JsonResult JsonResponse(this Controller controller, bool ok)
            => controller.Json(new JsonResponse(ok));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object, with serialized success marker and message.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="ok">Success marker.</param>
        /// <param name="message">Message to attach to result.</param>
        /// <inheritdoc cref="JsonResponse(Controller, bool)"/>
        public static JsonResult JsonResponse(this Controller controller, bool ok, string message)
            => controller.Json(new JsonResponse(ok, message));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object, with serialized success marker, message and data.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="ok">Success marker.</param>
        /// <param name="message">Message to attach to result.</param>
        /// <param name="data">Data object to append to result.</param>
        /// <inheritdoc cref="JsonResponse(Controller, bool, string)"/>
        public static JsonResult JsonResponse(this Controller controller, bool ok, string message, object data)
            => controller.Json(new JsonResponse(ok, message, data));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object, with serialized success marker and data.
        /// </summary>
        /// <inheritdoc cref="JsonResponse(Controller, bool, string, object)"/>
        public static JsonResult JsonResponse(this Controller controller, bool ok, object data)
            => controller.Json(new JsonResponse(ok, data));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing successful response,
        /// with serialized success marker equals <see langword="true"/>.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <returns>An instance of <see cref="JsonResult"/>.</returns>
        public static JsonResult JsonSuccess(this Controller controller)
            => controller.JsonResponse(ok: true);

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing successful response,
        /// with serialized success marker equals <see langword="true"/> and message.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="message">Message to attach to result.</param>
        /// <inheritdoc cref="JsonSuccess(Controller)"/>
        public static JsonResult JsonSuccess(this Controller controller, string message)
            => controller.JsonResponse(ok: true, message: message);

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing successful response,
        /// with serialized success marker equals <see langword="true"/>, message and data.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="message">Message to attach to result.</param>
        /// <param name="data">Data object to append to result.</param>
        /// <inheritdoc cref="JsonSuccess(Controller, string)"/>
        public static JsonResult JsonSuccess(this Controller controller, string message, object data)
            => controller.JsonResponse(ok: true, message: message, data: data);

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/>.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <returns>An instance of <see cref="JsonResult"/>.</returns>
        public static JsonResult JsonError(this Controller controller)
            => controller.JsonResponse(ok: false);

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/> and message.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="message">Message to attach to result.</param>
        /// <inheritdoc cref="JsonError(Controller)"/>
        public static JsonResult JsonError(this Controller controller, string message)
            => controller.JsonResponse(ok: false, message: message);

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/>, message and data.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="message">Message to attach to result.</param>
        /// <param name="data">Data object to append to result.</param>
        /// <inheritdoc cref="JsonError(Controller, string)"/>
        public static JsonResult JsonError(this Controller controller, string message, object data)
            => controller.JsonResponse(ok: false, message: message, data: data);

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/> and error messages.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="messages">List of error messages.</param>
        /// <returns>An instance of <see cref="JsonResult"/>.</returns>
        public static JsonResult JsonError(this Controller controller, IEnumerable<string> messages)
            => controller.Json(new JsonResponse(errorMessages: messages));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/>, error messages and data.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/>.</param>
        /// <param name="messages">List of error messages.</param>
        /// <param name="data">Data object to append to result.</param>
        /// <inheritdoc cref="JsonError(Controller, IEnumerable{string})"/>
        public static JsonResult JsonError(this Controller controller, IEnumerable<string> messages, object data)
            => controller.Json(new JsonResponse(errorMessages: messages, data: data));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/> and errors.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/></param>
        /// <param name="errors">List of errors.</param>
        /// <returns>An instance of <see cref="JsonResult"/>.</returns>
        public static JsonResult JsonError(this Controller controller, IEnumerable<JsonResponseError> errors)
            => controller.Json(new JsonResponse(errors: errors));

        /// <summary>
        /// Creates a <see cref="JsonResult"/> object representing failed response,
        /// with serialized success marker equals <see langword="false"/>, errors and data.
        /// </summary>
        /// <param name="controller">The <see cref="Controller"/></param>
        /// <param name="errors">List of errors.</param>
        /// <param name="data">Data object to append to result.</param>
        /// <inheritdoc cref="JsonError(Controller, IEnumerable{JsonResponseError})"/>
        public static JsonResult JsonError(this Controller controller, IEnumerable<JsonResponseError> errors, object data)
            => controller.Json(new JsonResponse(errors: errors, data: data));
    }
}
