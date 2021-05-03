using System;

namespace FinalYearProject.Services.Authentication
{
    public class AuthException : Exception
    {
        public AuthErrorType ErrorType { get; set; }

        public AuthException() : base()
        {
        }

        public AuthException(string message) : base(message)
        {
        }

        public AuthException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthException(AuthErrorType errorType) => ErrorType = errorType;
    }
}