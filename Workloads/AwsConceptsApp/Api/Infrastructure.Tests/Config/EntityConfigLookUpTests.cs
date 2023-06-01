using Infrastructure.Config;

namespace Infrastructure.Tests.Config
{
    public class EntityConfigLookUpTests
    {
        Dictionary<Type, EntityConfig> expectedConfigMap = new Dictionary<Type, EntityConfig>
        {
           
        };
        Dictionary<Type, EntityConfig> unexpectedConfigMap = new Dictionary<Type, EntityConfig>
        {
          
        };
        [Fact]
        public void GetConfigMap_ReturnsCorrectConfigMap()
        {

            // Act
            var configMap = EntityConfigLookUp.GetConfigMap().RepoConfig;

            //comparer.
            foreach (var expectedPair in expectedConfigMap)
            {
                var actualPair = configMap[expectedPair.Key];
                Assert.Equal(expectedPair.Value, actualPair, new EntitiesConfigComparer());
            }
        }

        [Fact]
        public void GetConfigMap_ReturnsCorrectConfigMap_Failure()
        {


            // Act
            var configMap = EntityConfigLookUp.GetConfigMap().RepoConfig;

            //comparer.
            foreach (var expectedPair in unexpectedConfigMap)
            {
                var actualPair = configMap[expectedPair.Key];
                Assert.NotEqual(expectedPair.Value, actualPair, new EntitiesConfigComparer());
            }
        }
    }


}
