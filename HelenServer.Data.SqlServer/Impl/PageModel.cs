namespace HelenServer.Data.SqlServer
{
    [Serializable]
    public class PageModel<T> : IPageModel<T>
    {
        public static readonly IPageModel<T> Empty = new PageModel<T>();

        private IReadOnlyList<T> _data = Array.Empty<T>();
        private int _pageIndex = 1;
        private int _pageSize = 10;
        private long _totalCount;

        public IReadOnlyList<T> Data
        {
            get => _data;
            set
            {
                if (value is not null)
                {
                    _data = value;
                }
            }
        }

        public int PageIndex
        {
            get => _pageIndex;
            set
            {
                if (value > 0)
                {
                    _pageIndex = value;
                }
            }
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0)
                {
                    _pageSize = value;
                }
            }
        }

        public long TotalCount
        {
            get => _totalCount;
            set
            {
                if (value > 0)
                {
                    _totalCount = value;
                }
            }
        }

        public int PageCount => (int)((_totalCount + _pageSize - 1) / _pageSize);

        public T this[int index] => _data[index];

        public int Count => _data.Count;
    }
}