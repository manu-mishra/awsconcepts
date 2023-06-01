using Infrastructure.Repository;

namespace Infrastructure.Tests.Repository
{
    public class StoredEntityTests
    {
        private readonly object _domainEntity = new object();
        private readonly DateTime _now = DateTime.UtcNow;
        private readonly string _createdBy = "John Doe";
        private readonly string _lastUpdatedBy = "Jane Doe";

        [Fact]
        public void StoredEntity_WithFullConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var storedEntity = new StoredEntity<object>(_domainEntity, _now, _createdBy, _now, _lastUpdatedBy);

            // Assert
            Assert.Equal(_domainEntity, storedEntity.Entity);
            Assert.Equal(_now, storedEntity.CreatedAt);
            Assert.Equal(_createdBy, storedEntity.CreatedBy);
            Assert.Equal(_now, storedEntity.LastUpdatedAt);
            Assert.Equal(_lastUpdatedBy, storedEntity.LastUpdatedBy);
        }

        [Fact]
        public void StoredEntity_WithPartialConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var storedEntity = new StoredEntity<object>(_domainEntity, _now, _lastUpdatedBy);

            // Assert
            Assert.Equal(_domainEntity, storedEntity.Entity);
            Assert.Null(storedEntity.CreatedAt);
            Assert.Null(storedEntity.CreatedBy);
            Assert.Equal(_now, storedEntity.LastUpdatedAt);
            Assert.Equal(_lastUpdatedBy, storedEntity.LastUpdatedBy);
        }

        [Fact]
        public void StoredEntity_WithDefaultConstructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var storedEntity = new StoredEntity<Object>();

            // Assert
            Assert.Null(storedEntity.Entity);
            Assert.Null(storedEntity.CreatedAt);
            Assert.Null(storedEntity.CreatedBy);
            Assert.Null(storedEntity.LastUpdatedAt);
            Assert.Null(storedEntity.LastUpdatedBy);
        }
    }
}
