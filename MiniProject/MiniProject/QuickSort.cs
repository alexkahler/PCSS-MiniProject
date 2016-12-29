namespace Server
{
    class QuickSort
    {
        public WordCount[] Sort(WordCount[] array)
        {
            var sorted = (WordCount[])array.Clone();
            Sort(sorted, 0, sorted.Length);
            return sorted;
        }

        private void Sort(WordCount[] array, int left, int right)
        {
            if (left < right)
            {
                int boundary = left;
                for (int i = left + 1; i < right; i++)
                {
                    if (array[i].Count > array[left].Count)
                    {
                        Swap(array, i, ++boundary);
                    }
                }
                Swap(array, left, boundary);
                Sort(array, left, boundary);
                Sort(array, boundary + 1, right);
            }
        }
        private void Swap(WordCount[] array, int first, int second)
        {
            WordCount tmp = array[first];
            array[first] = array[second];
            array[second] = tmp;
        }
    }
    class WordCount
    {
        public WordCount(string word, int count)
        {
            Word = word;
            Count = count;
        }
        public string Word;
        public int Count;
    }
}
