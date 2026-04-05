namespace CR.DtoBase
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public long TotalItems { get; set; }
    }
}
