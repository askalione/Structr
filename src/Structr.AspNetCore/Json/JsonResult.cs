using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.Json
{
    public class JsonResult
    {
        public bool Ok { get; }
        private readonly string _message;
        public string Message => Ok ? _message : _errors.FirstOrDefault()?.Message;
        public object Data { get; }
        private readonly List<JsonError> _errors = new List<JsonError>();
        public IEnumerable<JsonError> Errors => _errors;

        public JsonResult(bool ok)
        {
            Ok = ok;
        }

        public JsonResult(bool ok, string message) : this(ok)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            if (ok)
                _message = message;
            else
                _errors.Add(new JsonError(message));
        }

        public JsonResult(bool ok, object data) : this(ok)
        {
            Data = data;
        }

        public JsonResult(bool ok, string message, object data) : this(ok, message)
        {
            Data = data;
        }

        public JsonResult(IEnumerable<JsonError> errors) : this(false)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            _errors = errors.ToList();
        }

        public JsonResult(IEnumerable<JsonError> errors, object data) : this(false, errors)
        {
            Data = data;
        }

        public JsonResult(IEnumerable<string> errorsMessages) : this(false)
        {
            if (errorsMessages == null)
                throw new ArgumentNullException(nameof(errorsMessages));

            _errors = errorsMessages.ToList().ConvertAll(x => new JsonError(x));
        }

        public JsonResult(IEnumerable<string> errorsMessages, object data) : this(false, errorsMessages)
        {
            Data = data;
        }
    }
}