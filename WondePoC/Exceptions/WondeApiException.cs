using System;

namespace WondePoC.Exceptions
{
    /// <summary>
    /// Thrown when the Wonde API returns a non-success HTTP status or an unexpected response.
    /// </summary>
    public class WondeApiException : Exception
    {
        public int? HttpStatusCode { get; }

        public WondeApiException(string message)
            : base(message) { }

        public WondeApiException(string message, int httpStatusCode)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public WondeApiException(string message, Exception inner)
            : base(message, inner) { }

        public WondeApiException(string message, int httpStatusCode, Exception inner)
            : base(message, inner)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
