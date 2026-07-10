using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; }
        public ErrorType? ErrorType { get; }

        protected Result(bool isSuccess, string? error, ErrorType? errorType)
        {
            IsSuccess = isSuccess;
            Error = error;
            ErrorType = errorType;
        }

        public static Result Success() => new(true, null, null);

        public static Result Failure(string error, ErrorType? errorType = null) => new(false, error, errorType);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, T? value, string? error, ErrorType? errorType)
            : base(isSuccess, error, errorType)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(true, value, null, null);

        public new static Result<T> Failure(string error, ErrorType? errorType = null) => new(false, default, error, errorType);
    }
}