namespace CustomExceptionHandler
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;

    /// <summary>
    /// class containing information throw custom exception from the user or the business logic exceptions
    /// </summary>
    [DataContract]
    public class CustomException : Exception
    {

        /// <summary>
        /// List of errors occured
        /// </summary>
        [DataMember]
        public List<string> Errors { get; set; }


        /// <summary>
        /// level of exception
        /// </summary>
        [DataMember]
        public ExceptionLevel ExceptionLevel { get; set; }

        /// <summary>
        /// error code of occured error
        /// </summary>
        [DataMember]
        public HttpStatusCode ErrorStatusCode { get; set; } = HttpStatusCode.BadRequest;

        /// <summary>
        /// initialize the list
        /// </summary>
        public CustomException()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// add if error messages are sent as input
        /// </summary>
        /// <param name="errorMessages">list of error messages to throw</param>
        /// <param name="errorCode">error code</param>
        public CustomException(List<string> errorMessages, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            Errors = errorMessages;
            ErrorStatusCode = errorCode;
        }


        /// <summary>
        /// add if error messages are sent as input
        /// </summary>
        /// <param name="errorMessage">error message to throw</param>
        /// <param name="errorCode">error code</param>
        public CustomException(string errorMessage, HttpStatusCode errorCode = HttpStatusCode.BadRequest)
        {
            Errors = new List<string>() { errorMessage };
            ErrorStatusCode = errorCode;
        }

        /// <summary>
        /// Add a new error
        /// </summary>
        /// <param name="error">new error</param>
        public void AddError(string error)
        {
            Errors.Add(error);
        }

        /// <summary>
        /// list of all error messages
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllErrorMessages()
        {
            return Errors;
        }

    }


    /// <summary>
    /// exception levels
    /// </summary>
    [DataContract]
    public enum ExceptionLevel
    {
        [DataMember]
        None = 0,
        [DataMember]
        Warning = 1,
        [DataMember]
        Error = 2,
        [DataMember]
        Exception = 3
    }
}
