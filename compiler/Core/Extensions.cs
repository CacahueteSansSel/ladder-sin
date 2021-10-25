namespace compiler.Core
{
    public static class Extensions
    {
        public static int IndexOf<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(value)) return i;
            }

            return -1;
        }
    }
}