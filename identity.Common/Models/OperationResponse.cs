namespace identity.Common.Models;

public class OperationResponse
{
    public string? Message { get; set; }
    public bool? Result { get; set; }
    public string? Token { get; set; }
    public OperationResponse() { }
    public OperationResponse(bool result, string? message)
    {
        Result = result;
        Message = message;
    }
    public OperationResponse(bool result, string? message, string token)
    {
        Result = result;
        Message = message;
        Token = token;
    }
}
