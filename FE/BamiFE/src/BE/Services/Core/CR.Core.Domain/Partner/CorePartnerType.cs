using CR.Constants.Common.Database;
using CR.EntitiesBase.Interfaces;

namespace CR.Core.Domain.Partner;

/// <summary>
/// Loại đối tác
/// </summary>
///
[Table(nameof(CorePartnerType), Schema = DbSchemas.CRCore)]
public class CorePartnerType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên loại đối tác
    /// </summary>
    ///
    [MaxLength(512)]
    public required string Name { get; set; }
    #region Link với đối tác
    public List<CorePartner> Partners { get; } = [];
    #endregion
}
