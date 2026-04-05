using Microsoft.AspNetCore.Mvc;

namespace CR.DtoBase
{
    public class PagingRequestBaseDto
    {
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "pageNumber")]
        public int PageNumber { get; set; } = 1;

        private string? _keyword { get; set; }

        [FromQuery(Name = "keyword")]
        public string? Keyword
        {
            get => _keyword;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _keyword = null;
                }
                else
                {
                    _keyword = value.Trim();
                }
            }
        }

        public int GetSkip()
        {
            int skip = (PageNumber - 1) * PageSize;
            if (skip < 0)
            {
                skip = 0;
            }
            return skip;
        }

        public List<string> Sort { get; set; } = new();
    }
}
