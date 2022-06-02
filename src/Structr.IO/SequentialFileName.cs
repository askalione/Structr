using System;
using System.IO;

namespace Structr.IO
{
    /// <summary>
    /// Provides methods for generate new sequntial filename based on guid and time stamp segments
    /// using different params, e.g. existing file, MIME type or file extension.
    /// </summary>
    public static class SequentialFileName
    {
        /// <summary>
        /// Generates new sequntial filename based on guid and time stamp segments.
        /// </summary>
        public static string NewFileName()
        {
            string guidSegment = Guid.NewGuid().ToString("N");
            string dateSegment = DateTime.Now.ToString("yyyyMMddHHmmss");

            string result = $"{dateSegment}-{guidSegment}";
            return result;
        }

        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments
        /// using extension from exists file name.
        /// </summary>
        /// <param name="existsFileName">Name of exists file.</param>
        public static string NewFileName(string existsFileName)
            => NewFileNameWithExtension(Path.GetExtension(existsFileName));

        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments
        /// with specified MIME type.
        /// </summary>
        /// <param name="mimeType">MIME type.</param>
        public static string NewFileNameWithMimeType(string mimeType)
            => NewFileNameWithExtension(MimeTypeHelper.GetExtension(mimeType));

        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments
        /// with specified extension.
        /// </summary>
        /// <param name="extension">File extension (starts with dot, e.g. ".pdf")</param>
        /// <exception cref="ArgumentNullException">If <paramref name="extension"/> is <see langword="null"/> or empty.</exception>
        public static string NewFileNameWithExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                throw new ArgumentNullException(nameof(extension));
            }

            string formattedExt = extension;
            if (formattedExt.StartsWith(".") == false)
            {
                formattedExt = "." + formattedExt;
            }

            string result = $"{NewFileName()}{formattedExt}";
            return result;
        }
    }
}
