namespace CR.EntitiesBase.Interfaces;

/// <summary>
/// Nhận request id từ X-Request-Id
/// </summary>
public interface IRequestId
{
    /// <summary>
    /// X-Request-Id trong header
    /// </summary>
    string RequestId { get; set; }
}
