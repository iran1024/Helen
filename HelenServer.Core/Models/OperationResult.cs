namespace HelenServer.Core
{
    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; } = default!;

        public static OperationResult<T> OK(T data)
        {
            return OK(data, "");
        }

        public static OperationResult<T> OK(
            T data, string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            return new OperationResult<T>()
            {
                Status = OperationStatus.Success,
                Message = message,
                Data = data
            };
        }

        public static new OperationResult<T> Failed(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            return new OperationResult<T>()
            {
                Status = OperationStatus.Failed,
                Message = message
            };
        }

        public static new OperationResult<T> Error(Exception exception)
        {
            var result = new OperationResult<T>()
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


    public class OperationResult
    {
        public OperationStatus Status { get; set; }

        public bool Succeed => Status == OperationStatus.Success;

        public string Message { get; set; } = string.Empty;

        public static OperationResult OK()
        {
            return OK("");
        }

        public static OperationResult OK(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            return new OperationResult()
            {
                Status = OperationStatus.Success,
                Message = message
            };
        }

        public static OperationResult Failed(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            return new OperationResult()
            {
                Status = OperationStatus.Failed,
                Message = message
            };
        }

        public static OperationResult Error(Exception exception)
        {
            var result = new OperationResult()
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