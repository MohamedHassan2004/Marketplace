namespace Marketplace.Services.DTOs.Auth;
public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string ErrorMessage { get; set; }

    public static LoginResult Success(string token, string refreshToken)
        => new LoginResult { IsSuccess = true, Token = token, RefreshToken = refreshToken };

    public static LoginResult Fail(string error)
        => new LoginResult { IsSuccess = false, ErrorMessage = error };
}


public class RegisterResult
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public string UserId { get; set; }

    public static RegisterResult Success(string message, string userId)
        => new RegisterResult { IsSuccess = true, UserId = userId };

    public static RegisterResult Fail(string error)
        => new RegisterResult { IsSuccess = false, ErrorMessage = error };
}

public class TokenResult
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string ErrorMessage { get; set; }

    public static TokenResult Success(string token, string refreshToken)
        => new TokenResult { IsSuccess = true, Token = token, RefreshToken = refreshToken };

    public static TokenResult Fail(string error)
        => new TokenResult { IsSuccess = false, ErrorMessage = error };
}

