namespace Infomatrix.Core.Application.Constants;

public static class CacheKeys
{
    public static string SignUp(string email)
        => $"sign_up_password:{email}";

    public static string ResetPasswordToken(string email)
        => $"reset_password:token:{email}";
}
