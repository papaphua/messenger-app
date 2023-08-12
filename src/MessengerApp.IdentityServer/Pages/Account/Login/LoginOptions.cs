namespace MessengerApp.IdentityServer.Pages.Account.Login;

public class LoginOptions
{
    public static bool AllowLocalLogin = true;
    public static bool AllowRememberLogin = true;
    public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
    public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    public static string SignupLink = "/Account/Signup?ReturnUrl=%2FAccount%2FLogin";
}