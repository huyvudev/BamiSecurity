using CR.EntitiesBase.Base;
using CR.EntitiesBase.Interfaces;

namespace CR.EntitiesBase.Entities
{
    public interface IRolePermission<TRoleId> : IEntity<int>, ICreatedBy
    {
        TRoleId RoleId { get; set; }
        string PermissionKey { get; set; }
    }
}
