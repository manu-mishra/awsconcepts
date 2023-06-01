namespace Infrastructure.Config
{
    internal class EntityConfigLookUp
    {
        EntityConfigLookUp()
        {
            RepoConfig = new Dictionary<Type, EntityConfig>();
            Configure();
        }
        public Dictionary<Type, EntityConfig> RepoConfig { get; private set; }
        internal static EntityConfigLookUp GetConfigMap()
        {
            EntityConfigLookUp config = new EntityConfigLookUp();
            return config;
        }

        private void Configure()
        {
            // Template for unit testing
            RepoConfig.Add(typeof(Domain.Template),
                new EntityConfig("Id", "IdentityId", pkPrefix: "P_K#", skPrefix: "S_K#"));
            // Template for unit testing
            RepoConfig.Add(typeof(Domain.TemplateSecondary),
                new EntityConfig("Id", "IdentityId", pkPrefix: "P_K#", skPrefix: "S_K#", isProjectedEntity:true));
        }
    }


}
