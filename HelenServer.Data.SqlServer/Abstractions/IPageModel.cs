namespace HelenServer.Data.SqlServer
{
    public interface IPageModel<out T>
    {
        IReadOnlyList<T> Data { get; }

        int Count { get; }

        int PageIndex { get; }

        int PageSize { get; }

        long TotalCount { get; set; }

        int PageCount { get; }
    }
}
