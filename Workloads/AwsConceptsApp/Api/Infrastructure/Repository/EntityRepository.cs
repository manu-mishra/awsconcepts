using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Application.Common.Exceptions;
using Application.Identity;
using Application.Interfaces;
using Infrastructure.Config;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repository
{
    internal class EntityRepository<DomainEntity> : IEntityRepository<DomainEntity>
    {
        private readonly IAmazonDynamoDB dynamoDB;
        private readonly IIdentity identity;
        private readonly string pkPropertyName;
        private readonly string pkPrefix;
        private readonly string skPropertyName;
        private readonly string skPrefix;
        private readonly bool isProjectedEntity;

        public EntityRepository(IAmazonDynamoDB dynamoDB, EntityConfigLookUp repositoryConfigLookUp, IIdentity identity)
        {
            this.dynamoDB = dynamoDB;
            this.identity = identity;
            EntityConfig config = repositoryConfigLookUp.RepoConfig[typeof(DomainEntity)]; ;
            pkPropertyName = config.PkPropertyName;
            pkPrefix = config.PkPrefix;
            skPropertyName = config.SkPropertyName;
            skPrefix = config.SkPrefix;
            isProjectedEntity = config.IsProjectedEntity;
        }
        public async Task Delete(string EntityId, string ScopeId, CancellationToken CancellationToken)
        {
            if (isProjectedEntity)
                throw new InvalidOperationException($"could not perform {nameof(Delete)} on {typeof(DomainEntity)} using secondary index");
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = "awsconcepts",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "ek", new AttributeValue { S = pkPrefix + EntityId} },
                    { "sk", new AttributeValue { S = skPrefix + ScopeId} }
                },
                ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
            };
            DeleteItemResponse response = await dynamoDB.DeleteItemAsync(request, CancellationToken);
        }

        public async Task<IStoredEntity<DomainEntity>?> Get(string EntityId, string ScopeId, CancellationToken CancellationToken)
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = "awsconcepts",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "ek", new AttributeValue { S = pkPrefix + EntityId} },
                    { "sk", new AttributeValue { S = skPrefix + ScopeId} }
                },
                ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL
            };

            GetItemResponse response = await dynamoDB.GetItemAsync(request, CancellationToken);
            TraceConsumedCapacity(response.ConsumedCapacity);
            if (response.Item.Count == 0)
            {
                return default;
            }

            Document itemAsDocument = Document.FromAttributeMap(response.Item);
            return JsonSerializer.Deserialize<StoredEntity<DomainEntity>>(itemAsDocument.ToJson());
        }

        public async Task<(List<IStoredEntity<DomainEntity>>, string?)> GetAll(string ScopeId, string? ContinuationToken, CancellationToken CancellationToken)
        {
            List<IStoredEntity<DomainEntity>> result = new List<IStoredEntity<DomainEntity>>();
            string? continuationToken = default;

            QueryRequest query = GetScopedQuery(ScopeId);
            if (!string.IsNullOrEmpty(ContinuationToken))
            {
                query.ExclusiveStartKey = JsonSerializer.Deserialize<Dictionary<string, AttributeValue>>(Encoding.UTF8.GetString(Convert.FromBase64String(ContinuationToken)));
            }

            QueryResponse response = await dynamoDB.QueryAsync(query, CancellationToken);
            TraceConsumedCapacity(response.ConsumedCapacity);
            foreach (Dictionary<string, AttributeValue>? item in response.Items)
            {
                Document itemAsDocument = Document.FromAttributeMap(item);
                IStoredEntity<DomainEntity>? entity = JsonSerializer.Deserialize<StoredEntity<DomainEntity>>(itemAsDocument.ToJson());
                if (entity is not null)
                    result.Add(entity);
            }
            //dbresponse.las
            if (response.LastEvaluatedKey.Count() > 0)
            {
                continuationToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response.LastEvaluatedKey)));
            }
            return (result, continuationToken);
        }

        public async Task<IStoredEntity<DomainEntity>> Create(DomainEntity DomainEntity, CancellationToken CancellationToken)
        {
            if (isProjectedEntity)
                throw new InvalidOperationException($"could not perform {nameof(Put)} on {typeof(DomainEntity)} using secondary index");
            StoredEntity<DomainEntity> storageEntity = new StoredEntity<DomainEntity>(DomainEntity, DateTime.UtcNow, identity.Id, DateTime.UtcNow, identity.Id);

            string entityJson = JsonSerializer.Serialize(storageEntity);

            Document entityDocument = Document.FromJson(entityJson);
            Dictionary<string, AttributeValue> entityAsAttibute = entityDocument.ToAttributeMap();

            entityAsAttibute["ek"] = new AttributeValue { S = pkPrefix + entityAsAttibute["Entity"].M[pkPropertyName].S };
            entityAsAttibute["sk"] = new AttributeValue { S = skPrefix + entityAsAttibute["Entity"].M[skPropertyName].S };
            entityAsAttibute["etype"] = new AttributeValue { S = typeof(DomainEntity).ToString() };

            PutItemRequest putItemRequest = new PutItemRequest
            {
                TableName = "awsconcepts",
                Item = entityAsAttibute,
                ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL,
                ConditionExpression = "attribute_not_exists(sk)"
            };

            PutItemResponse response = await dynamoDB.PutItemAsync(putItemRequest, CancellationToken);
            TraceConsumedCapacity(response.ConsumedCapacity);

            return (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                ? storageEntity
                : throw new RepositoryException($"DynamoDB failure", entityJson, response.HttpStatusCode, response.ResponseMetadata) { };
        }
        public async Task<IStoredEntity<DomainEntity>> Update(DomainEntity DomainEntity, CancellationToken CancellationToken)
        {
            if (isProjectedEntity)
                throw new InvalidOperationException($"could not perform {nameof(Put)} on {typeof(DomainEntity)} using secondary index");
            StoredEntity<DomainEntity> storageEntity = new StoredEntity<DomainEntity>(DomainEntity, DateTime.UtcNow, identity.Id, DateTime.UtcNow, identity.Id);

            string entityJson = JsonSerializer.Serialize(storageEntity);

            Document entityDocument = Document.FromJson(entityJson);
            Dictionary<string, AttributeValue> entityAsAttibute = entityDocument.ToAttributeMap();

            entityAsAttibute["ek"] = new AttributeValue { S = pkPrefix + entityAsAttibute["Entity"].M[pkPropertyName].S };
            entityAsAttibute["sk"] = new AttributeValue { S = skPrefix + entityAsAttibute["Entity"].M[skPropertyName].S };
            entityAsAttibute["etype"] = new AttributeValue { S = typeof(DomainEntity).ToString() };

            PutItemRequest putItemRequest = new PutItemRequest
            {
                TableName = "awsconcepts",
                Item = entityAsAttibute,
                ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL,
                ConditionExpression = "attribute_exists(sk)"
            };

            PutItemResponse response = await dynamoDB.PutItemAsync(putItemRequest, CancellationToken);
            TraceConsumedCapacity(response.ConsumedCapacity);

            return (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                ? storageEntity
                : throw new RepositoryException($"DynamoDB failure", entityJson, response.HttpStatusCode, response.ResponseMetadata) { };
        }

        private void TraceConsumedCapacity(ConsumedCapacity ConsumedCapacity)
        {
            System.Diagnostics.Trace.WriteLine(ConsumedCapacity);
        }

        private QueryRequest GetScopedQuery(string ScopeId)
        {
            if (isProjectedEntity)
            {
                return new QueryRequest()
                {
                    TableName = "awsconcepts",
                    KeyConditionExpression = "ek = :v_ek",
                    ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL,
                    //Limit = 5,
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                     {":v_ek", new AttributeValue{S = pkPrefix + ScopeId} }
                }
                };
            }
            return new QueryRequest()
            {
                TableName = "awsconcepts",
                IndexName = "sk-pk-index",
                KeyConditionExpression = "sk = :v_sk",
                ReturnConsumedCapacity = ReturnConsumedCapacity.TOTAL,
                //Limit = 5,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                     {":v_sk", new AttributeValue{S = skPrefix + ScopeId} }
                }
            };
        }
    }
}
