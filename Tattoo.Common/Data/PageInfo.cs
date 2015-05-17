namespace Tattoo.Common.Data
{
    public class PageInfo
    {
        public PageInfo()
        {
            PageNumber = 1;
            PageSize = Constants.DefaultPageSize;
        }

        public PageInfo(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int Skip
        {
            get { return (PageNumber - 1) * PageSize; }
        }
    }
}