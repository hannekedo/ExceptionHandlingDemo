using System;

namespace ExceptionHandlingDemo.ExceptionHandling
{
    public class BaseCustomException : Exception
    {
        public string ErrorCode { get; }
        public string ObjectType { get; }

        public BaseCustomException(string code) : base()
        {
            ErrorCode = code;
        }

        public BaseCustomException(string code, string message) : base(message)
        {
            ErrorCode = code;
        }

        public BaseCustomException(string code, string objectType, string message) : base(message)
        {
            ErrorCode = code;
            ObjectType = objectType;
        }
    }
}