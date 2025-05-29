namespace Lab_9
{
    public class Helpers
    {
        public static void AppendToArray<T>(ref T[] array, T element)
        {
            array = array.Append(element).ToArray();
        }
        public static void AppendToArray<T>(ref T[] array, T[] otherArray)
        {
            array = array.Concat(otherArray).ToArray();
        }
    }

    public class Program
    {
        public static void Main() { }
    }
}
