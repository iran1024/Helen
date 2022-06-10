namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> objs, Action<T> action)
        {
            foreach (var o in objs)
            {
                action(o);
            }
        }
    }
}