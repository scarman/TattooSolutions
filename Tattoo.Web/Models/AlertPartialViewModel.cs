#region Using Directives

using Tattoo.Web.Core.Enumerations;

#endregion

namespace Tattoo.Web.Models
{
    public class AlertPartialViewModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AlertPartialViewModel(string message)
        {
            Message = message;
            AlertType = BootstrapStyle.Info;
            Subject = string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AlertPartialViewModel(string message, BootstrapStyle alertType)
        {
            AlertType = alertType;
            Message = message;
            Subject = string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public AlertPartialViewModel(string message, string subject, BootstrapStyle alertType)
        {
            AlertType = alertType;
            Subject = subject;
            Message = message;
        }

        public BootstrapStyle AlertType { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
    }
}