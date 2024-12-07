namespace Identity.Application.ViewModels.Account;

public class LoginAccountRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DeviceName { get; set; }
}