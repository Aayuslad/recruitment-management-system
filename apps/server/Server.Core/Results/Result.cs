namespace Server.Core.Results
{
    /// <summary>
    /// Non-generic result used for operations that don't return a value.
    /// </summary>
    public sealed class Result
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }

        private Result(bool isSuccess, string? errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static Result Success(int statusCode = 200) => new(true, default, statusCode);
        public static Result Failure(string errorMessage, int statusCode = 500) => new(false, errorMessage, statusCode);
    }

    /// <summary>
    /// Generic result that carries a value on success.
    /// </summary>
    public sealed class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }

        public Result(bool isSuccess, T? value, string? errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value, int statusCode = 200) => new(true, value, null, statusCode);
        public static Result<T> Failure(string error, int statusCode = 500) => new(false, default, error, statusCode);
    }
}