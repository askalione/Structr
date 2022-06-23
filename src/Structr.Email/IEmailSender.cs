using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    /// <summary>
    /// Provides functionality for sending an emails.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an <see cref="EmailMessage"/>.
        /// </summary>
        /// <param name="email">The <see cref="EmailMessage"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendEmailAsync(EmailMessage email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email rendering from a template.
        /// </summary>
        /// <param name="email">The <see cref="EmailTemplateMessage"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendEmailAsync(EmailTemplateMessage email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email rendering from a template with model <see cref="{TModel}"/>.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="email">The <see cref="EmailTemplateMessage{TModel}"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendEmailAsync<TModel>(EmailTemplateMessage<TModel> email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email rendering from a template file.
        /// </summary>
        /// <param name="email">The <see cref="EmailTemplateFileMessage"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendEmailAsync(EmailTemplateFileMessage email, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email rendering from a template file with model <see cref="{TModel}"/>.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="email">The <see cref="EmailTemplateFileMessage{TModel}"/>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is None.</param>
        Task SendEmailAsync<TModel>(EmailTemplateFileMessage<TModel> email, CancellationToken cancellationToken = default);
    }
}
