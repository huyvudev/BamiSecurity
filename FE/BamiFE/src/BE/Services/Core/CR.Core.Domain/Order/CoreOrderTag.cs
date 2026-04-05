using CR.Constants.Common.Database;

namespace CR.Core.Domain.Order;

[Table(nameof(CoreOrderTag), Schema = DbSchemas.CRCore)]
public class CoreOrderTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int TagId { get; set; }

    public CoreTag Tag { get; } = null!;
}
