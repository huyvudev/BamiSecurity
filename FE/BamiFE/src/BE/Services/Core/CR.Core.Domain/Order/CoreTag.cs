using CR.Constants.Common.Database;

namespace CR.Core.Domain.Order;

[Table(nameof(CoreTag), Schema = DbSchemas.CRCore)]
public class CoreTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(512)]
    public string Name { get; set; } = null!;
}