using CR.EntitiesBase.Base;
using CR.EntitiesBase.Interfaces;

namespace CR.EntitiesBase.Entities
{
    public interface IUserRole<TUserId, TRoleId> : IEntity<int>, IFullAudited
    {
        TUserId UserId { get; set; }
        TRoleId RoleId { get; set; }
    }
}
