namespace App.utils
{
    public class Result<T, S>
    {
        public T? Value { get; protected set; }
        public List<S>? Errors { get; protected set; }
        public bool IsSuccess => Value != null;
        public bool IsFailure => Errors != null;
        private Result(T value)
        {
            Value = value;
            Errors = default;
        }
        private Result(List<S> errors)
        {
            Value = default;
            Errors = errors;
        }
        public static implicit operator Result<T, S>(T value) => new(value);
        public static implicit operator Result<T, S>(List<S> errors) => new(errors);
        public static Result<T, S> OnSuccess(T value) => new(value);
        public static Result<T, S> OnFailure(List<S> errors) => new(errors);
    }
}
