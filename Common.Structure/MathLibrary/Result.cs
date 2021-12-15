namespace Common.Structure.MathLibrary
{
    public sealed class Result
    {
        public static Result<T> ErrorResult<T>(string error)
        {
            return new Result<T>(error);
        }
    }
}
