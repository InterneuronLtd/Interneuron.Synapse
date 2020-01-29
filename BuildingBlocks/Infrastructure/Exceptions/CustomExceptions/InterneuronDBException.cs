using System;
using Npgsql;

namespace Interneuron.Infrastructure.CustomExceptions
{
    [Serializable]
    public class InterneuronDBException: Exception
    {
        public string ErrorId { get; private set; }

        public short ErrorCode { get; private set; }

        public string ErrorType { get; private set; }

        public string ErrorMessage { get; private set; }
        public string ErrorResponseMessage { get; private set; }
        public InterneuronDBException(short errorCode = 500, string errorMessage = "", string errorType = "System Error", string errorId = null): base($"Http.{errorCode} {errorType} {errorMessage}")
        {
            this.ErrorId = errorId?? Guid.NewGuid().ToString();
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorType = errorType;
            this.ErrorResponseMessage = $"Error fetching data from database. Please check the error log with ID: {this.ErrorId} for more details";
        }

        public InterneuronDBException(Exception innerException, short errorCode = 500, string errorMessage = "", string errorType = "System Error",  string errorId = null) : base($"Http.{errorCode} {errorType} {errorMessage}", innerException)
        {
            this.ErrorId = errorId ?? Guid.NewGuid().ToString();
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorType = errorType;
            this.ErrorResponseMessage = $"Error fetching data from database. Please check the error log with ID: {this.ErrorId} for more details";
        }

        public InterneuronDBException(PostgresException npgEx, short errorCode = 500, string errorMessage = "", string errorType = "System Error",  string errorId = null) : base($"Http.{errorCode} {errorType} {errorMessage}", npgEx)
        {
            this.ErrorId = errorId ?? Guid.NewGuid().ToString();
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorType = errorType;
            this.ErrorResponseMessage = $"Error fetching data from database. Please check the error log with ID: {this.ErrorId} for more details";
        }
    }
}
