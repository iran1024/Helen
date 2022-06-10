namespace HelenServer.Core
{
    public class Operation<TParameter> : OperationUser
    {
        public TParameter Parameter { get; set; }

        public Operation(OperationUser user) : base(user)
        {
            Parameter = default!;
        }

        public Operation(OperationUser user, TParameter parameter) : base(user)
        {
            Parameter = parameter;
        }

        public Operation<TResult> To<TResult>()
        {
            if (Parameter is TResult parameter)
            {
                return new Operation<TResult>(this, parameter);
            }
            else
            {
                throw new InvalidCastException(typeof(TResult).FullName);
            }
        }

        public Operation<TProperty> To<TProperty>(Func<TParameter, TProperty> selector)
        {
            return new Operation<TProperty>(this, selector(Parameter));
        }
    }

    public class Operation<TKey, TParameter> : OperationUser
    {
        public Operation(OperationUser user) : base(user)
        {
            Key = default!;
            Parameter = default!;
        }

        public Operation(OperationUser user, TKey key, TParameter parameter) : base(user)
        {
            Key = key;
            Parameter = parameter;
        }

        public TKey Key { get; set; }

        public TParameter Parameter { get; set; }
    }
}
