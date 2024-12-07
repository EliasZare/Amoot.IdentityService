namespace Identity.Domain.Common;

public class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
}

