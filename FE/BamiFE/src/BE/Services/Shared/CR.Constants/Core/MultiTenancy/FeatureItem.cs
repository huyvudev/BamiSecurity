namespace CR.Constants.Core.MultiTenancy
{
    /// <summary>
    /// Chi tiết một feature
    /// </summary>
    public class FeatureItem
    {
        public Features Id { get; set; }
        public string Name { get; set; }
        public Features? ParentId { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="id">Id định danh</param>
        /// <param name="name">Tên sẽ được localize khi trả ra ngoài</param>
        public FeatureItem(Features id, string name)
        {
            Id = id;
            Name = name;
        }

        public FeatureItem(Features id, string name, Features? parentId)
            : this(id, name)
        {
            ParentId = parentId;
        }
    }
}
