using System;
using System.IO;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing data about an email attachment.
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Email attachment content.
        /// </summary>
        public Stream? Content { get; }

        /// <summary>
        /// File name.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Content type.
        /// </summary>
        public string? ContentType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAttachment"/> class with the <paramref name="fileName"/>.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="fileName"/> is <see langword="null"/> or empty.</exception>
        public EmailAttachment(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            FileName = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAttachment"/> class with the <paramref name="fileName"/> and the <paramref name="contentType"/>.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="fileName"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="contentType"/> is <see langword="null"/> or empty.</exception>
        public EmailAttachment(string fileName, string contentType)
            : this(fileName)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            ContentType = contentType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAttachment"/> class with the <paramref name="content"/>, the <paramref name="fileName"/> and the <paramref name="contentType"/>.
        /// </summary>
        /// <param name="content">The <see cref="Stream"/>.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="contentType">The content type.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="fileName"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="contentType"/> is <see langword="null"/> or empty.</exception>
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
