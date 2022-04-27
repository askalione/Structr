using System;
using System.IO;

namespace Structr.Email
{
    public class EmailAttachment
    {
        public Stream? Content { get; }
        public string FileName { get; }        
        public string? ContentType { get; }

        public EmailAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            FileName = fileName;
        }

        public EmailAttachment(string fileName, string contentType)
            : this(fileName)
        {            
            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }
            
            ContentType = contentType;
        }

        public EmailAttachment(Stream content, string fileName, string contentType)
            : this(fileName, contentType)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Content = content;
        }
    }
}
