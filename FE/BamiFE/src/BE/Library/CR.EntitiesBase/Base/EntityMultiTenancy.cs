using CR.EntitiesBase.Interfaces;

namespace CR.EntitiesBase.Base
{
    /// <summary>
    /// Đối tượng cho thuê PM
    /// </summary>
    public abstract class EntityMultiTenancy : IMultiTenancy
    {
        public virtual int TenantId { get; set; }
    }
}
