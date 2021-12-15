namespace Common.Structure.MathLibrary
{
    public sealed class Result<T>
    {
        public T Value
        {
            get;
        }

        public string Error
        {
            get;
        }

        public Result(T value)
        {
            Value = value;
        }

        public Result(string error)
        {
            Error = error;
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }

        public bool IsError()
        {
            return !string.IsNullOrEmpty(Error);
        }
    }
}
