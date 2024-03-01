namespace Domain.Shared;

public interface IAuditable
{
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
}

public interface ISoftDelete
{
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }
    public void Restore();
}

public class BaseAuditableEntity : IAuditable, ISoftDelete
{
    public DateTime? CreatedAt { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int? DeletedBy { get; set; }

    public void Restore()
    {
        DeletedBy = null;
        DeletedAt = null;
    }
}