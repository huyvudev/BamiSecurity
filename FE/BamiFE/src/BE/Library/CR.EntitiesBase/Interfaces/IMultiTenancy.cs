namespace CR.EntitiesBase.Interfaces
{
    /// <summary>
    /// Đối tượng cho thuê PM
    /// </summary>
    public interface IMultiTenancy
    {
        /// <summary>
        /// Id đối tượng cho thuê
        /// </summary>
        public int TenantId { get; set; }
    }
}
