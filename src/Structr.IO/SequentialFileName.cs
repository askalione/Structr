using System;
using System.IO;

namespace Structr.IO
{
    public static class SequentialFileName
    {
        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments.
        /// </summary>
        /// <returns></returns>
        public static string NewFileName()
        {
            string guidSegment = Guid.NewGuid().ToString("N");
            string dateSegment = DateTime.Now.ToString("yyyyMMddHHmmss");

            return $"{dateSegment}-{guidSegment}";
        }

        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments
        ///     using extension from exists file name.
        /// </summary>
        /// <param name="existsFileName">Name of exists file.</param>
        /// <returns></returns>
        public static string NewFileName(string existsFileName)
        {
            string extension = Path.GetExtension(existsFileName);
            return string.IsNullOrWhiteSpace(extension) == false
                ? NewFileNameWithExtension(extension)
                : NewFileName();
        }

        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments
        ///     with specified MIME type.
        /// </summary>
        /// <param name="mimeType">MIME type.</param>
        /// <returns></returns>
        public static string NewFileNameWithMimeType(string mimeType)
        {
            return NewFileNameWithExtension(MimeTypeHelper.GetExtension(mimeType));
        }

        /// <summary>
        /// Generate new sequntial filename based on guid and time stamp segments
        ///     with specified extension.
        /// </summary>
        /// <param name="extension">Extension (starts with dot e.g. (".exe"))</param>
        /// <returns></returns>
        public static string NewFileNameWithExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException(nameof(extension));

            var formattedExt = extension;

            if (!formattedExt.StartsWith("."))
                formattedExt = "." + formattedExt;

            return $"{NewFileName()}{formattedExt}";
        }
    }
}
