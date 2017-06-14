using System;

namespace Videpa.Identity.Api.ExceptionHandling.ExceptionResults
{
    public static class RequiredActions
    {
        public const string RefreshCollection = "refresh-collection";
        public const string RefreshEntity = "refresh-entity";
    }

    public class ErrorResponseMessage
    {
        public string Message { get; set; }
        public string DeveloperHints { get; set; }
        public string RequestId { get; set; }
        public int HttpStatusCode { get; set; }
        public DateTimeOffset Time { get; set; }

        public string RequiredAction { get; set; }

        /// <summary>
        /// Used for Upgrade Required
        /// </summary>
        public string Action { get; set; }
        public string Link { get; set; }
    }
}