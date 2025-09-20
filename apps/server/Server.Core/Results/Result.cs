namespace Server.Core.Results
{
    /// <summary>
    /// Non-generic result used for operations that don't return a value.
    /// </summary>
    public sealed class Result
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }

        private Result(bool isSuccess, string? errorMessage)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        public static Result Success() => new(true, default);
        public static Result Failure(string errorMessage) => new(false, errorMessage);
    }

    /// <summary>
    /// Generic result that carries a value on success.
    /// </summary>
    public sealed class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? ErrorMessage { get; private set; }

        public Result(bool isSuccess, T? value, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
    }
}