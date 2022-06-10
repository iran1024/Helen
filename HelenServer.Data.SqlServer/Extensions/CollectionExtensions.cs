namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> @this)
        {
            return @this == null || !@this.Any();
        }
    }
}