using Application.Interfaces;

namespace Application.Tests.Implementations
{
    public class StoredEntity<DomainEntity> : IStoredEntity<DomainEntity>
    {
        public StoredEntity()
        {

        }
        public StoredEntity(DomainEntity entity, DateTime createdDateTime, string createdBy, DateTime lastUpdatedDateTime, string lastUpdatedBy)
        {
            Entity = entity;
            CreatedAt = createdDateTime;
            CreatedBy = createdBy;
            LastUpdatedAt = lastUpdatedDateTime;
            LastUpdatedBy = lastUpdatedBy;
        }
        public StoredEntity(DomainEntity entity, DateTime lastUpdatedDateTime, string lastUpdatedBy)
        {
            Entity = entity;
            LastUpdatedAt = lastUpdatedDateTime;
            LastUpdatedBy = lastUpdatedBy;
        }
        public DomainEntity Entity { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }
    }
}
