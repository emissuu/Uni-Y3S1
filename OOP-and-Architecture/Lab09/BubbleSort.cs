namespace Lab9;

public class BubbleSort
{
    /// <summary>
    /// Comparer for bubble sorting function  <br />
    /// (a, b) => a &gt; b will sort in ascending order 
    /// </summary>
    public delegate bool Compare(int a, int b);

    public static IList<int> Sort(IList<int> array, Compare compare)
    {
        int temp;
        for (int i = 0; i < array.Count; i++)
        {
            for (int j = 0; j < array.Count - i - 1; j++)
            {
                if (compare(array[j], array[j + 1]))
                {
                    temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
        return array;
    }
}