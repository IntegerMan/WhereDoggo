namespace MattEland.Util
{
    public static class RandomHelper
    {

        public static T? GetRandomElement<T>(this IList<T>? items, Random random)
        {
            if (items == null || items.Count == 0)
            {
                return default;
            }

            int index = random.Next(0, items.Count);

            return items[index];
        }

    }
}