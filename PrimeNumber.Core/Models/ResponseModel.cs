namespace PrimeNumber.Core.Models;

public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string Code { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
}

public class ResponseModel<T>
{
    public bool IsSuccess { get; set; }
    public string Code { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
    public T? Data { get; set; }
}

public class ResponseModelSuccess<T> : ResponseModel<T> where T : class
{
    public ResponseModelSuccess(T model, string message = "")
    {
        IsSuccess = true;
        Message = message;
        Data = model;
        Code = string.Empty;
    }
}

public class ResponseModelSuccess : ResponseModel
{
    public ResponseModelSuccess(string message = "")
    {
        IsSuccess = true;
        Message = message;
        Code = string.Empty;
    }
}

public class ResponseModelError<T> : ResponseModel<T> where T : class
{
    public ResponseModelError(string errorMessage, T? data = null, string code = "")
    {
        Message = errorMessage;
        IsSuccess = false;
        Code = code;
        Data = data;
    }
}

public class ResponseModelError : ResponseModel
{
    public ResponseModelError(string errorMessage, string code = "")
    {
        Message = errorMessage;
        IsSuccess = false;
        Code = code;
    }
}