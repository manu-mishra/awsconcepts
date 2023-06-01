using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Application.Interfaces;
using Infrastructure.Config;
using Infrastructure.Repository;
using OpenSearch.Client;
using System.Net;

namespace Infrastructure.Tests.Repository
{
    public class EntitiesRepositoryTests
    {
        private readonly string _entityId = "TestId";
        private readonly string _scopeId = "TestUserId";
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly EntityConfigLookUp configMap = EntityConfigLookUp.GetConfigMap();
        private EntityConfig _entityConfig => configMap.RepoConfig[typeof(Template)];
        [Fact(DisplayName = "Delete_WithValidInput")]
        public async Task Delete_WithValidInput_CallsDynamoDbDeleteItemAsync()
        {
            // Arrange
            var mockDynamoDb = new Mock<IAmazonDynamoDB>();
            var mockIdentity = new Mock<IIdentity>();
            var repository = new EntityRepository<Template>(mockDynamoDb.Object, configMap, mockIdentity.Object);

            // Act
            await repository.Delete(_entityId, _scopeId, _cancellationToken);

            // Assert
            mockDynamoDb.Verify(x => x.DeleteItemAsync(It.Is<DeleteItemRequest>(r =>
                r.TableName == "awsconcepts" &&
                r.Key["ek"].S == _entityConfig.PkPrefix + _entityId &&
                r.Key["sk"].S == _entityConfig.SkPrefix + _scopeId &&
                r.ReturnConsumedCapacity == ReturnConsumedCapacity.TOTAL
            ), _cancellationToken), Times.Once);
        }
        [Fact(DisplayName = "Get_WithValidInput")]
        public async Task Get_WithValidInput_CallsDynamoDbGetItemAsync()
        {
            // Arrange
            var mockDynamoDb = new Mock<IAmazonDynamoDB>();
            var mockIdentity = new Mock<IIdentity>();

            var repository = new EntityRepository<Template>(mockDynamoDb.Object, configMap, mockIdentity.Object);

            var storedEntity = new StoredEntity<Template>(
                new Template() { Id = _entityId, IdentityId = _scopeId },
                DateTime.Now, _entityId, DateTime.Now, _scopeId);
            var storedEntityJson = JsonSerializer.Serialize(storedEntity);
            var itemAsDocument = Document.FromJson(storedEntityJson);
            var response = new GetItemResponse
            {
                Item = itemAsDocument.ToAttributeMap()
            };
            mockDynamoDb.Setup(x => x.GetItemAsync(It.IsAny<GetItemRequest>(), _cancellationToken))
                .ReturnsAsync(response);

            // Act
            var result = await repository.Get(_entityId, _scopeId, _cancellationToken);

            // Assert
            mockDynamoDb.Verify(x => x.GetItemAsync(It.Is<GetItemRequest>(r =>
                r.TableName == "awsconcepts" &&
                r.Key["ek"].S == _entityConfig.PkPrefix + _entityId &&
                r.Key["sk"].S == _entityConfig.SkPrefix + _scopeId &&
                r.ReturnConsumedCapacity == ReturnConsumedCapacity.TOTAL)
                , _cancellationToken), Times.Once);
            Assert.Equal(storedEntity.CreatedAt, result?.CreatedAt);
            Assert.Equal(storedEntity.LastUpdatedAt, result?.LastUpdatedAt);
            Assert.Equal(storedEntity.CreatedBy, result?.CreatedBy);
            Assert.Equal(storedEntity.LastUpdatedBy, result?.LastUpdatedBy);
        }

        [Fact(DisplayName = "GetAll_Primary_WithValidInput")]
        public async Task GetAll_Primary_WithValidInput_CallsDynamoDbQueryAsync()
        {
            // Arrange
            var mockDynamoDb = new Mock<IAmazonDynamoDB>();
            var mockIdentity = new Mock<IIdentity>();
            var repository = new EntityRepository<Template>(mockDynamoDb.Object, configMap, mockIdentity.Object);
            var continuationToken = "";

            var storedEntity1 = new StoredEntity<Template>(
                new Template() { Id = "TestId1", IdentityId = _scopeId },
                DateTime.Now, "TestCreatedBy1", DateTime.Now, "TestLastUpdatedBy1");
            var storedEntity2 = new StoredEntity<Template>(
                new Template() { Id = "TestId2", IdentityId = _scopeId },
                DateTime.Now, "TestCreatedBy2", DateTime.Now, "TestLastUpdatedBy2");
            var storedEntityList = new List<IStoredEntity<Template>> { storedEntity1, storedEntity2 };
            var response = new QueryResponse
            {
                Items = new List<Dictionary<string, AttributeValue>>
        {
            Document.FromJson(JsonSerializer.Serialize(storedEntity1)).ToAttributeMap(),
            Document.FromJson(JsonSerializer.Serialize(storedEntity2)).ToAttributeMap()
        },
                LastEvaluatedKey = new Dictionary<string, AttributeValue>
        {
            { "ek", new AttributeValue{ S = "TestLastEvaluatedKey" } }
        }
            };

            mockDynamoDb.Setup(x => x.QueryAsync(It.IsAny<QueryRequest>(), _cancellationToken))
                .ReturnsAsync(response);

            // Act
            var result = await repository.GetAll(_scopeId, continuationToken, _cancellationToken);

            // Assert
            mockDynamoDb.Verify(x => x.QueryAsync(It.Is<QueryRequest>(r =>
                r.TableName == "awsconcepts" &&
                r.IndexName == "sk-pk-index" &&
                r.KeyConditionExpression == "sk = :v_sk" &&
                r.ExpressionAttributeValues[":v_sk"].S == _entityConfig.SkPrefix + _scopeId &&
                r.ReturnConsumedCapacity == ReturnConsumedCapacity.TOTAL
            ), _cancellationToken), Times.Once);
            
            var invocations = mockDynamoDb.Invocations;
        }

        [Fact(DisplayName = "GetAll_Secondary_WithValidInput")]
        public async Task GetAll_Secondary_WithValidInput_CallsDynamoDbQueryAsync()
        {
            // Arrange
            var mockDynamoDb = new Mock<IAmazonDynamoDB>();
            var mockIdentity = new Mock<IIdentity>();
            var repository = new EntityRepository<TemplateSecondary>(mockDynamoDb.Object, configMap, mockIdentity.Object);
            var continuationToken = "";

            var storedEntity1 = new StoredEntity<TemplateSecondary>(
                new TemplateSecondary() { Id = "TestId1", IdentityId = _scopeId },
                DateTime.Now, "TestCreatedBy1", DateTime.Now, "TestLastUpdatedBy1");
            var storedEntity2 = new StoredEntity<TemplateSecondary>(
                new TemplateSecondary() { Id = "TestId2", IdentityId = _scopeId },
                DateTime.Now, "TestCreatedBy2", DateTime.Now, "TestLastUpdatedBy2");
            var storedEntityList = new List<IStoredEntity<TemplateSecondary>> { storedEntity1, storedEntity2 };
            var response = new QueryResponse
            {
                Items = new List<Dictionary<string, AttributeValue>>
        {
            Document.FromJson(JsonSerializer.Serialize(storedEntity1)).ToAttributeMap(),
            Document.FromJson(JsonSerializer.Serialize(storedEntity2)).ToAttributeMap()
        },
                LastEvaluatedKey = new Dictionary<string, AttributeValue>
        {
            { "ek", new AttributeValue{ S = "TestLastEvaluatedKey" } }
        }
            };

            mockDynamoDb.Setup(x => x.QueryAsync(It.IsAny<QueryRequest>(), _cancellationToken))
                .ReturnsAsync(response);

            // Act
            var result = await repository.GetAll(_scopeId, continuationToken, _cancellationToken);

            // Assert
            mockDynamoDb.Verify(x => x.QueryAsync(It.Is<QueryRequest>(r =>
                r.TableName == "awsconcepts" &&
                r.KeyConditionExpression == "ek = :v_ek" &&
                r.ExpressionAttributeValues[":v_ek"].S == _entityConfig.PkPrefix + _scopeId &&
                r.ReturnConsumedCapacity == ReturnConsumedCapacity.TOTAL
            ), _cancellationToken), Times.Once);
            var invocations = mockDynamoDb.Invocations;
        }

        [Fact(DisplayName = "Create_WithValidInput")]
        public async Task Create_WithValidInput_CallsDynamoDbPutItemAsync()
        {
            // Arrange
            var mockDynamoDb = new Mock<IAmazonDynamoDB>();
            var mockIdentity = new Mock<IIdentity>();
            mockIdentity.Setup(x => x.Id).Returns(_scopeId);
            var repository = new EntityRepository<Template>(mockDynamoDb.Object, configMap, mockIdentity.Object);
            var entity = new Template() { Id = _entityId, IdentityId = _scopeId };
            var putItemResponse = new PutItemResponse { HttpStatusCode = HttpStatusCode.OK };
            mockDynamoDb.Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), _cancellationToken))
                .ReturnsAsync(putItemResponse);

            // Act
            var result = await repository.Create(entity, _cancellationToken);

            // Assert
            mockDynamoDb.Verify(x => x.PutItemAsync(It.Is<PutItemRequest>(r =>
                r.TableName == "awsconcepts" &&
                r.Item["ek"].S == _entityConfig.PkPrefix + _entityId &&
                r.Item["sk"].S == _entityConfig.SkPrefix + _scopeId &&
                r.Item["etype"].S == typeof(Template).ToString() &&
                r.ReturnConsumedCapacity == ReturnConsumedCapacity.TOTAL &&
                r.ConditionExpression == "attribute_not_exists(sk)" // Ensures creates if SK does not already exist
            ), _cancellationToken), Times.Once);
            Assert.Equal(entity.Id, result.Entity.Id);
            Assert.Equal(entity.IdentityId, result.Entity.IdentityId);
            Assert.Equal(mockIdentity.Object.Id, result.CreatedBy);
            Assert.Equal(mockIdentity.Object.Id, result.LastUpdatedBy);
            //verify that CreatedAt and LastUpdatedAt are not null
            Assert.True(result.CreatedAt.HasValue);
            Assert.True(result.LastUpdatedAt.HasValue);
        }

        [Fact(DisplayName = "Update_WithValidInput")]
        public async Task Update_WithValidInput_CallsDynamoDbPutItemAsync()
        {
            // Arrange
            var mockDynamoDb = new Mock<IAmazonDynamoDB>();
            var mockIdentity = new Mock<IIdentity>();
            var repository = new EntityRepository<Template>(mockDynamoDb.Object, configMap, mockIdentity.Object);
            var domainEntity = new Template() { Id = _entityId, IdentityId = _scopeId };
            
            var storageEntity = new StoredEntity<Template>(domainEntity, DateTime.Now, _entityId, DateTime.Now, _scopeId);
            var storageEntityJson = JsonSerializer.Serialize(storageEntity);
            var itemAsDocument = Document.FromJson(storageEntityJson);
            var itemAsAttribute = itemAsDocument.ToAttributeMap();
            itemAsAttribute["ek"] = new AttributeValue { S = _entityConfig.PkPrefix + itemAsAttribute["Entity"].M[_entityConfig.PkPropertyName].S };
            itemAsAttribute["sk"] = new AttributeValue { S = _entityConfig.SkPrefix + itemAsAttribute["Entity"].M[_entityConfig.SkPropertyName].S };
            itemAsAttribute["etype"] = new AttributeValue { S = typeof(Template).ToString() };
            var putItemResponse = new PutItemResponse
            {
                HttpStatusCode = HttpStatusCode.OK
            };
            mockDynamoDb.Setup(x => x.PutItemAsync(It.IsAny<PutItemRequest>(), _cancellationToken))
                .ReturnsAsync(putItemResponse);

            // Act
            var result = await repository.Update(domainEntity, _cancellationToken);
            // Assert
            mockDynamoDb.Verify(x => x.PutItemAsync(It.Is<PutItemRequest>(r =>
                r.TableName == "awsconcepts" &&
                r.Item["ek"].S == _entityConfig.PkPrefix + _entityId &&
                r.Item["sk"].S == _entityConfig.SkPrefix + _scopeId &&
                r.Item["etype"].S == typeof(Template).ToString() &&
                r.ReturnConsumedCapacity == ReturnConsumedCapacity.TOTAL &&
                r.ConditionExpression == "attribute_exists(sk)" // Ensure that cant update if not already exist.
            ), _cancellationToken), Times.Once);
           // Assert.Equal(result.CreatedAt, result.LastUpdatedAt);
            Assert.Equal(result.CreatedBy, result.LastUpdatedBy);
            Assert.Equal(result.Entity, domainEntity);
        }
    }
}
