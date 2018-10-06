
using System.Linq;

static class ArrayHelper
{
    public static bool IsArrayInJArray(int[] searchArray, int[][] JArray) //JArray = JaggedArray
    {
        foreach(int[] subArray in JArray)
        {
            if(Enumerable.SequenceEqual(subArray, searchArray))
                return true;
        }
        return false;
    }

    //public static bool isArrayInJList()

    public static int[][] AddJArrayToJArray(int[][] arr1, int[][] arr2)
    {
        int[][] temporaryArr = new int[arr1.Length + arr2.Length][];
        for (int i = 0; i < arr1.Length; i++)
        {
            if (arr1[i] != null)
                temporaryArr[i] = arr1[i];

        }
        for (int j = 0; j < arr2.Length; j++)
        {
            if (arr2[j] != null)
                temporaryArr[j + arr1.Length] = arr2[j];
        }
        temporaryArr = temporaryArr.Where(c => c != null).ToArray();
        return temporaryArr;
    }

}

