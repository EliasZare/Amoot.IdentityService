namespace Identity.Domain.Entities;

public class Account : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public List<JwtToken> Tokens { get; set; }
}