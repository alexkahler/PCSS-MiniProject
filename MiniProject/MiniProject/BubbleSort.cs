namespace Server
{
    class BubbleSort
    {
        public static string[] Sort(string[] array)
        {
            int length = array.Length;

            string temp = array[0];

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if(array[i].CompareTo(array[j]) > 0) 
                    {
                        temp = array[i];

                        array[i] = array[j];

                        array[j] = temp;
                    }

                }

            }

            return array;
        }
    }
}