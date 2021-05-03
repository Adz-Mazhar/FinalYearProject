using System;

namespace FinalYearProject.Services.Database
{
    public class DatabaseException : Exception
    {
        public DatabaseErrorType ErrorType { get; set; }

        public DatabaseException() : base()
        {
        }

        public DatabaseException(string message) : base(message)
        {
        }

        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DatabaseException(DatabaseErrorType errorType) => ErrorType = errorType;
    }
}
