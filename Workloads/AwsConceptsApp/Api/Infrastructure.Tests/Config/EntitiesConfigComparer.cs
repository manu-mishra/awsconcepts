using Infrastructure.Config;

namespace Infrastructure.Tests.Config
{
    internal class EntitiesConfigComparer : IEqualityComparer<EntityConfig>
    {
        public bool Equals(EntityConfig? x, EntityConfig? y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else
                return x.PkPrefix.Equals(y.PkPrefix)
                    && x.SkPrefix.Equals(y.SkPrefix)
                    && x.PkPropertyName.Equals(y.PkPropertyName)
                    && x.SkPropertyName.Equals(y.SkPropertyName)
                    && x.IsProjectedEntity == y.IsProjectedEntity;
        }

        public int GetHashCode(EntityConfig obj)
        {
            return obj.PkPrefix.GetHashCode()
                ^ obj.PkPropertyName.GetHashCode()
                ^ obj.SkPrefix.GetHashCode()
                ^ obj.SkPropertyName.GetHashCode()
                ^ obj.IsProjectedEntity.GetHashCode();
        }
    }
}
