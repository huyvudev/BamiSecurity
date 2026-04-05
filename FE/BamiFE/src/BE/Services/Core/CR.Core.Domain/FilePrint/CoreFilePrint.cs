using CR.Constants.Common.Database;
using CR.Constants.Core.Order;
using CR.Core.Domain.Order;

namespace CR.Core.Domain.FilePrint;

[Table(nameof(CoreFilePrint), Schema = DbSchemas.CRCore)]
public class CoreFilePrint
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Đường dẫn file
    /// </summary>
    [MaxLength(512)]
    public required string UrlFile { get; set; }

    /// <summary>
    /// Trạng thái file
    /// </summary>
    public FilePrintStatus Status { get; set; }

    public int OrderItemId { get; set; }

    /// <summary>
    /// File này nằm trong OrderItem nào
    /// </summary>
    public CoreOrderItem OrderItem { get; set; } = null!;
}
