public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}

public class LoginResult : OperationResult
{
    public string Token { get; set; }
    public static LoginResult Success(string message = null, string token = null) =>
        new() { IsSuccess = true, ErrorMessage = message, Token = token };
    public static LoginResult Fail(string message) =>
        new() { IsSuccess = false, ErrorMessage = message };
}

public class RegisterResult : OperationResult
{
    public string UserId { get; set; }
    public static RegisterResult Success(string message = null, string userId = null) =>
        new() { IsSuccess = true, ErrorMessage = message, UserId = userId };
    public static RegisterResult Fail(string message) =>
    new() { IsSuccess = false, ErrorMessage = message };

}
