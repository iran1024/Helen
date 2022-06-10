namespace HelenServer.Core
{
    public interface IDataIndexer
    {
        object IndexOf(Type type, object instance, int index);
    }

    public interface IDataIndexer<TModel> : IDataIndexer
    {
        TModel IndexOf();
    }
}