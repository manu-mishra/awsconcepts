namespace Infrastructure.Config
{
    internal class EntityConfig
    {
        public EntityConfig(string pkPropertyName, string skPropertyName, bool isProjectedEntity = false, string? pkPrefix = default, string? skPrefix = default)
        {
            PkPropertyName = pkPropertyName;
            SkPropertyName = skPropertyName;
            IsProjectedEntity = isProjectedEntity;
            PkPrefix = pkPrefix ?? string.Empty;
            SkPrefix = skPrefix ?? string.Empty;
        }
        public string PkPropertyName { get; set; }
        public string PkPrefix { get; set; }
        public string SkPropertyName { get; set; }
        public string SkPrefix { get; set; }
        public bool IsProjectedEntity { get; set; }
    }
}
