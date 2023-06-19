namespace PrimeNumber.Data.Entity;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedUser { get; set; }

}
