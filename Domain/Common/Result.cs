namespace Domain.Common
{
    public class Result
    {
        public bool IsFail => !IsCorrect;

        public bool IsCorrect => Error is null;

        public Error Error { get; }

        protected Result(Error error)
        {
            Error = error;
        }

        public static Result Correct()
        {
            return new Result(null);
        }

        public static Result Fail(Error error)
        {
            if (error is null)
                error = new Error(500, "Response mapping error, null error value");

            return new Result(error);
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value) : this(value, null)
        {
        }

        private Result(T value, Error error) : base(error)
        {
            Value = value;
        }

        public static Result<T> Correct(T value)
        {
            if (value is null)
            {
                Error error = new Error(500, "Response mapping error, null value");
                return Result<T>.Fail(value, error);
            }

            return new Result<T>(value);
        }

        public static Result<T> Fail(T value, Error error)
        {
            if (null == error)
                error = new Error(500, "Response mapping error, null error value");

            return new Result<T>(value, error);
        }

        public static implicit operator Result<T>(T value) => Result<T>.Correct(value);
    }

}