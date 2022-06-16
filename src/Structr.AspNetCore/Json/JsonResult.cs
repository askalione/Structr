using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Json
{
    /// <summary>
    /// Represents a result containing data object, errors list, message and success marker.
    /// </summary>
    public class JsonResult
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

        private readonly List<JsonError> _errors = new List<JsonError>();
        /// <summary>
        /// Returns list of errors contained in result.
        /// </summary>
        public IEnumerable<JsonError> Errors => _errors;

        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/>.
        /// </summary>
        /// <param name="ok">Identifies correctness of result. Should be <see langword="true"/> if result has no errors.</param>
        public JsonResult(bool ok)
        {
            Ok = ok;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with specified message.
        /// </summary>
        /// <inheritdoc cref="JsonResult(bool)"/>
        /// <param name="message">Message supplementing the result.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="message"/> is <see langword="null"/> or empty.</exception>
        public JsonResult(bool ok, string message) : this(ok)
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
                _errors.Add(new JsonError(message));
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with attached data.
        /// </summary>
        /// <inheritdoc cref="JsonResult(bool)"/>
        /// <param name="data">Attached data.</param>
        public JsonResult(bool ok, object data) : this(ok)
        {
            Data = data;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with specified message and attached data.
        /// </summary>
        /// <inheritdoc cref="JsonResult(bool, string)"/>
        /// <inheritdoc cref="JsonResult(bool, object)"/>
        public JsonResult(bool ok, string message, object data) : this(ok, message)
        {
            Data = data;
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with specified errors.
        /// </summary>
        /// <param name="errors">Errors contained in result.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="errors"/> is <see langword="null"/>.</exception>
        public JsonResult(IEnumerable<JsonError> errors) : this(false)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

            _errors = errors.ToList();
        }


        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with specified errors and attached data.
        /// </summary>
        /// <inheritdoc cref="JsonResult(IEnumerable{JsonError})"/>
        /// <inheritdoc cref="JsonResult(bool, object)"/>
        public JsonResult(IEnumerable<JsonError> errors, object data) : this(errors)
        {
            Data = data;
        }


        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with specified error messages.
        /// </summary>
        /// <param name="errorsMessages">Error messages contained in result.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="errorsMessages"/> is <see langword="null"/>.</exception>
        public JsonResult(IEnumerable<string> errorsMessages) : this(false)
        {
            if (errorsMessages == null)
            {
                throw new ArgumentNullException(nameof(errorsMessages));
            }

            _errors = errorsMessages.ToList().ConvertAll(x => new JsonError(x));
        }

        /// <summary>
        /// Creates an instance of <see cref="JsonResult"/> with specified error messages.
        /// </summary>
        /// <inheritdoc cref="JsonResult(IEnumerable{string})"/>
        /// <inheritdoc cref="JsonResult(bool, object)"/>
        public JsonResult(IEnumerable<string> errorsMessages, object data) : this(errorsMessages)
        {
            Data = data;
        }
    }
}