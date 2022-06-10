namespace HelenServer.Core
{
    public class PageResult<T> : OperationResult<List<T>>
    {
        public PageResult()
        {
            Data = new List<T>();
        }

        public PageResult(IPageOperation operational)
        {
            Num = operational.Num;

            Size = operational.Size;

            Data = new List<T>();
        }

        public PageResult(
            IPageOperation operational, long total, List<T> items) : this(operational)
        {
            Status = OperationStatus.Success;

            Total = total;

            Data = items;
        }

        public int Num { get; set; }
        public int Size { get; set; }
        public long Total { get; set; }

        public static PageResult<T> OK(
            IPageOperation operational, long total, List<T> items)
        {
            return new PageResult<T>(operational, total, items);
        }

        public static PageResult<T> NotSuccess(OperationResult sourceResult)
        {
            return new()
            {
                Status = sourceResult.Status,
                Message = sourceResult.Message,
            };
        }

        public static new PageResult<T> Failed(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            return new PageResult<T>()
            {
                Status = OperationStatus.Failed,
                Message = message
            };
        }

        public static new PageResult<T> Error(Exception exception)
        {
            var result = new PageResult<T>()
            {
                Status = OperationStatus.Error,
                Message = exception.Message
            };

#if DEBUG
            result.Message = exception.ToString();
#endif
            return result;
        }
    }
}
