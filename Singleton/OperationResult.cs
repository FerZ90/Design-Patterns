public enum Result
{
    None,
    Success,
    Error,
    Warning
}
public class OperationResult
{
    public Result Result { get; }
    public string Message { private set; get; }

    public object Data { private set; get; }

    private OperationResult(Result result)
    {
        Result = result;
    }

    public OperationResult()
    {
        Result = Result.None;
    }

    public static OperationResult CreateSuccessOperation() => new(Result.Success);

    public static OperationResult CreateWarningOperation(string msg) =>
        new(Result.Warning) { Message = msg };

    public static OperationResult CreateErrorOperation(string msg) =>
        new(Result.Error) { Message = msg };

    public static OperationResult CreateResultOperation(Result result, object data) => new(result) { Data = data };
}