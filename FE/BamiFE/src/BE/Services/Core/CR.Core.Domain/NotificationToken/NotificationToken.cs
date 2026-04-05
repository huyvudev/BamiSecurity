using CR.Constants.Common.Database;
using CR.Core.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Domain.AuthToken
{
    [Table(nameof(NotificationToken), Schema = DbSchemas.Default)]
    [Index(nameof(UserId), Name = $"IX_{nameof(NotificationToken)}")]
    public class NotificationToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        [Unicode(false)]
        public string? FcmToken { get; set; }

        [MaxLength(128)]
        [Unicode(false)]
        [Obsolete("bỏ chỉ cần dùng FcmToken")]
        public string? ApnsToken { get; set; }
        public int UserId { get; set; }
        public User User { get; } = null!;
    }
}
