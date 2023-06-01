namespace Application.Interfaces
{
    public interface IEntitySearchProvider
    {
        public Task<List<T>> SearchInScopeDomainEntity<T>(string SearchString, string scope, string scopeName) where T : class;
        public Task<List<T>> SearchWithNoScopeDomainEntity<T>(string SearchString) where T : class;
        public Task<List<T>> SearchParaphraseInScopeDomainEntity<T>(string SearchString, string scope, string scopeName, string fieldsName) where T : class;
        public Task<List<T>> SearchParaphraseWithNoScopeDomainEntity<T>(string SearchString, string fieldsName) where T : class;

    }
}
