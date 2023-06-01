using Application.Interfaces;
using Infrastructure.Config;
using OpenSearch.Client;

namespace Infrastructure.Search
{
    internal class OpenSearchProvider : IEntitySearchProvider
    {
        private readonly IOpenSearchClient searchClient;

        public OpenSearchProvider(EntityConfigLookUp RepositoryConfigLookUp, IOpenSearchClient SearchClient)
        {
            this.searchClient = SearchClient;
        }

        public async Task<List<T>> SearchInScopeDomainEntity<T>(string SearchString, string scope, string scopeName) where T : class
        {

            string searchQuery = @" {
            ""query"":
                {
                    ""bool"":
                    {
                       ""should"": 
                        [
                            {
                                ""multi_match"":{""query"":""#SearchTerm#""}
                            }
                        ],
                        ""filter"": {
                            ""term"":
                            {
                                ""#FieldName#"":""#FieldValue#""
                            }
                        }
                    }
                }
            }";
            searchQuery = searchQuery.Replace("#FieldName#", scopeName);
            searchQuery = searchQuery.Replace("#FieldValue#", scope);
            searchQuery = searchQuery.Replace("#SearchTerm#", SearchString);
            string searchIndex = typeof(T).ToString().ToLower();
            SearchResponse<T> searchResponse = await searchClient.LowLevel.SearchAsync<SearchResponse<T>>(searchIndex, searchQuery);
            return new List<T>(searchResponse.Documents);
        }

        public async Task<List<T>> SearchParaphraseInScopeDomainEntity<T>(string SearchString, string scope, string scopeName, string fieldsName) where T : class
        {

            string searchQuery = @" {
            ""query"":
                {
                    ""bool"":
                    {
                       ""should"": 
                        [
                            {
                                ""match_phrase"":{""#MatchPhraseFieldName#"":""#SearchTerm#""}
                            }
                        ],
                        ""filter"": {
                            ""term"":
                            {
                                ""#FieldName#"":""#FieldValue#""
                            }
                        }
                    }
                }
            }";
            searchQuery = searchQuery.Replace("#FieldName#", scopeName);
            searchQuery = searchQuery.Replace("#FieldValue#", scope);
            searchQuery = searchQuery.Replace("#SearchTerm#", SearchString);
            searchQuery = searchQuery.Replace("#MatchPhraseFieldName#", fieldsName);
            string searchIndex = typeof(T).ToString().ToLower();
            SearchResponse<T> searchResponse = await searchClient.LowLevel.SearchAsync<SearchResponse<T>>(searchIndex, searchQuery);
            return new List<T>(searchResponse.Documents);
        }
        public async Task<List<T>> SearchWithNoScopeDomainEntity<T>(string SearchString) where T : class
        {
            string searchQuery = @" {
            ""query"":
                {
                    ""multi_match"":
                    {
                        ""query"": ""#SearchTerm#""
                    }
                }
            }";

            searchQuery = searchQuery.Replace("#SearchTerm#", SearchString);
            string searchIndex = typeof(T).ToString().ToLower();
            SearchResponse<T> searchResponse = await searchClient.LowLevel.SearchAsync<SearchResponse<T>>(searchIndex, searchQuery);
            return new List<T>(searchResponse.Documents);
        }
        public async Task<List<T>> SearchParaphraseWithNoScopeDomainEntity<T>(string SearchString, string fieldsName) where T : class
        {
            string searchQuery = @" {
            ""query"":
                {
                    ""match_phrase"":
                    {
                        ""#MatchPhraseFieldName#"": ""#SearchTerm#""
                    }
                }
            }";

            searchQuery = searchQuery.Replace("#SearchTerm#", SearchString);
            searchQuery = searchQuery.Replace("#MatchPhraseFieldName#", fieldsName);
            string searchIndex = typeof(T).ToString().ToLower();
            SearchResponse<T> searchResponse = await searchClient.LowLevel.SearchAsync<SearchResponse<T>>(searchIndex, searchQuery);
            return new List<T>(searchResponse.Documents);
        }
    }
}
