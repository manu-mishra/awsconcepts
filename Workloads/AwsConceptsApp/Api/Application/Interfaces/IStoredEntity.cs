namespace Application.Interfaces
{
    public interface IStoredEntity<DomainEntity>
    {
        DomainEntity Entity { get; set; }
        DateTime? CreatedAt { get; set; }
        string? CreatedBy { get; set; }
        DateTime? LastUpdatedAt { get; set; }
        string? LastUpdatedBy { get; set; }
    }
}
