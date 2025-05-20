namespace Lab_9
{
    public class Helpers
    {
        public static void AppendToArray<T>(T[] array, T element)
        {
            array = array.Append(element).ToArray();
        }
    }
}
