namespace HelenServer.Core
{
    public interface IPageOperation
    {
        int Num { get; set; }
        int Size { get; set; }
        IDictionary<string, OrderByType>? Sorts { get; set; }
    }

    public class PageOperation<TParameter> : Operation<TParameter>, IPageOperation
    {
        public PageOperation(
            OperationUser user, int? num = null, int? size = null,
            IDictionary<string, OrderByType>? sort = null) : base(user)
        {
            Num = num is null or < 1 ? 1 : num.Value;

            Size = size is null or < 1 ? 10 : size.Value;

            Sorts = sort;
        }

        public int Num { get; set; }
        public int Size { get; set; }
        public IDictionary<string, OrderByType>? Sorts { get; set; }

        public new PageOperation<TProperty> To<TProperty>(Func<TParameter, TProperty> selector)
        {
            return new PageOperation<TProperty>(this, Num, Size, Sorts)
            {
                Parameter = selector(Parameter)
            };
        }
    }
}
