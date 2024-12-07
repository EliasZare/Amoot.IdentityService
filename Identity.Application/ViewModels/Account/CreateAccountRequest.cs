namespace Identity.Application.ViewModels.Account;

public class CreateAccountRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Password { get; set; }
}
