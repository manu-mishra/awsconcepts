namespace Application.Interfaces
{
    public interface IEntityRepository<DomainEntity>
    {
        Task<(List<IStoredEntity<DomainEntity>>, string?)> GetAll(string ScopeId, string? ContinuationToken, CancellationToken CancellationToken);
        Task<IStoredEntity<DomainEntity>?> Get(string EntityId, string ScopeId, CancellationToken CancellationToken);
        Task<IStoredEntity<DomainEntity>> Create(DomainEntity DomainEntity, CancellationToken CancellationToken);
        Task<IStoredEntity<DomainEntity>> Update(DomainEntity DomainEntity, CancellationToken CancellationToken);
        Task Delete(string EntityId, string ScopeId, CancellationToken CancellationToken);
    }
}
