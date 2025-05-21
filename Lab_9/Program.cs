namespace Lab_9
{
    public class Helpers
    {
        public static void AppendToArray<T>(T[] array, T element)
        {
            array = array.Append(element).ToArray();
        }
        public static void AppendToArray<T>(T[] array, T[] otherArray)
        {
            array = array.Concat(otherArray).ToArray();
        }
    }
}
