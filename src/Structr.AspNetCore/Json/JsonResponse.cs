using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Json
{
    /// <summary>
    /// Represents a result containing data object, errors list, message and success marker.
    /// </summary>
    public class JsonResponse
    {
        /// <summary>
        /// Returns <see langword="true"/> when there are no errors in result, otherwise <see langword="false"/>.
        /// </summary>
        public bool Ok { get; }

        private readonly string _message;
        /// <summary>
        /// Returns message supplementing the result. Contains text of specified message or first error's message or <see langword="null"/> in case of no errors.
        /// </summary>
        public string Message => Ok ? _message : _errors.FirstOrDefault()?.Message;

        /// <summary>
        /// Returns Data attached to result;
        /// </summary>
        public object Data { get; }

        private readonly List<JsonResponseError> _errors = new List<JsonResponseError>();
        /// <summary>
        /// Returns list of errors contained in result.
        /// </summary>
        public IEnumerable<JsonResponseError> Errors => _errors;

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/>.
        /// </summary>
        /// <param name="ok">Identifies correctness of result. Should be <see langword="true"/> if result has no errors.</param>
        public JsonResponse(bool ok)
        {
            Ok = ok;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with specified message.
        /// </summary>
        /// <inheritdoc cref="JsonResponse(bool)"/>
        /// <param name="message">Message supplementing the result.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is <see langword="null"/> or empty.</exception>
        public JsonResponse(bool ok, string message) : this(ok)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (ok)
            {
                _message = message;
            }
            else
            {
                _errors.Add(new JsonResponseError(message));
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with attached data.
        /// </summary>
        /// <inheritdoc cref="JsonResponse(bool)"/>
        /// <param name="data">Attached data.</param>
        public JsonResponse(bool ok, object data) : this(ok)
        {
            Data = data;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with specified message and attached data.
        /// </summary>
        /// <inheritdoc cref="JsonResponse(bool, string)"/>
        /// <inheritdoc cref="JsonResponse(bool, object)"/>
        public JsonResponse(bool ok, string message, object data) : this(ok, message)
        {
            Data = data;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with specified errors.
        /// </summary>
        /// <param name="errors">Errors contained in result.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="errors"/> is <see langword="null"/>.</exception>
        public JsonResponse(IEnumerable<JsonResponseError> errors) : this(false)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            _errors = errors.ToList();
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with specified errors and attached data.
        /// </summary>
        /// <inheritdoc cref="JsonResponse(IEnumerable{JsonResponseError})"/>
        /// <inheritdoc cref="JsonResponse(bool, object)"/>
        public JsonResponse(IEnumerable<JsonResponseError> errors, object data) : this(errors)
        {
            Data = data;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with specified error messages.
        /// </summary>
        /// <param name="errorMessages">Error messages contained in result.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="errorMessages"/> is <see langword="null"/>.</exception>
        public JsonResponse(IEnumerable<string> errorMessages) : this(false)
        {
            if (errorMessages == null)
            {
                throw new ArgumentNullException(nameof(errorMessages));
            }

            _errors = errorMessages.ToList().ConvertAll(x => new JsonResponseError(x));
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResponse"/> with specified error messages.
        /// </summary>
        /// <inheritdoc cref="JsonResponse(IEnumerable{string})"/>
        /// <inheritdoc cref="JsonResponse(bool, object)"/>
        public JsonResponse(IEnumerable<string> errorMessages, object data) : this(errorMessages)
        {
            Data = data;
        }
    }
}