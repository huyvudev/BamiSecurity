namespace CR.Constants.Core.MultiTenancy
{
    /// <summary>
    /// Danh sách các feature được thiết lập ngưỡng bằng đo đếm (<see cref="Features"/>)
    /// </summary>
    public class ListFeature
    {
        /// <summary>
        /// Danh sách chức năng, tên chức năng sẽ được localize ra ngoài
        /// </summary>
        public static readonly IEnumerable<FeatureItem> Items = new List<FeatureItem>()
        {
            new(Features.NumUser, "feature_NumUser"),
            new(Features.NumCourse, "feature_NumCourse"),
            new(Features.Storage, "feature_Storage"),
            new(Features.EditDomain, "feature_EditDomain"),
        };
    }
}
