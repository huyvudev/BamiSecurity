using CR.Constants.Common.Database;
using CR.Core.Domain.Batch;
using CR.EntitiesBase.Interfaces;

namespace CR.Core.Domain.Partner;

/// <summary>
/// Đối tác
/// </summary>
///
[Table(nameof(CorePartner), Schema = DbSchemas.CRCore)]
public class CorePartner : ICreatedBy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên đối tác
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    #region Link với loại đối tác
    /// <summary>
    /// Loại đối tác
    /// </summary>
    public int PartnerTypeId { get; set; }
    public CorePartnerType PartnerType { get; set; } = null!;

    #endregion

    public List<CoreBatch> Batches { get; set; } = [];
    #region audit
    public DateTime? CreatedDate { get; set; }
    public int? CreatedBy { get; set; }
    #endregion

}
