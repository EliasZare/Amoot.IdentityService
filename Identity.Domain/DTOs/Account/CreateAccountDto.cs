namespace Identity.Domain.DTOs.Account;

public class CreateAccountDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Password { get; set; }
}