using CR.EntitiesBase.Base;
using CR.EntitiesBase.Interfaces;

namespace CR.EntitiesBase.Entities
{
    public interface IUser : IEntity<int>, IFullAudited
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
    }
}
