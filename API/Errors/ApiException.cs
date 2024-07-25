namespace API.Errors;
public class ApiException(int statuscode, string message, string? detail)
{
public int StatusCode{get;set;} = statuscode;
public string Message {get;set;} = message;
public string? Detali {get;set;} = detail;
}