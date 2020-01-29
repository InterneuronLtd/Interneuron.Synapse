using System;

namespace Interneuron.Infrastructure.CustomExceptions
{
    [Serializable]
    public class InterneuronBusinessException : Exception
    {
        public string ErrorId { get; private set; }

        public short ErrorCode { get; private set; }

        public string ErrorType { get; private set; }

        public string ErrorMessage { get; private set; }

        public string ErrorResponseMessage { get; private set; }
        public InterneuronBusinessException(short errorCode = 500, string errorMessage = "", string errorType = "System Error", string errorId = null) : base($"Http.{errorCode}:- {errorType} - {errorMessage}")
        {
            this.ErrorId = errorId ?? Guid.NewGuid().ToString();
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorType = errorType;
            this.ErrorResponseMessage = $"Error invoking the API: {base.Message}. Please check the log with ID: {this.ErrorId} for more details";

        }

        public InterneuronBusinessException(Exception innerException, short errorCode = 500, string errorMessage = "", string errorType = "System Error", string errorId = null) : base($"Http.{errorCode}:- {errorType} - {errorMessage}", innerException)
        {
            this.ErrorId = errorId ?? Guid.NewGuid().ToString();
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
            this.ErrorType = errorType;
            this.ErrorResponseMessage = $"Error invoking the API: {base.Message} Please check the log with ID: {this.ErrorId} for more details";
        }
    }
}
