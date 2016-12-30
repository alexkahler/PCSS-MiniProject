using System.Collections.Generic;

namespace Server
{
    class BubbleSort
    {
        public static List<Bubble> Sort(List<Bubble> bubbleList)
        {
            int length = bubbleList.Count;

            Bubble temp = bubbleList[0];

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (bubbleList[i].Count < bubbleList[j].Count) 
                    {
                        temp = bubbleList[i];

                        bubbleList[i] = bubbleList[j];

                        bubbleList[j] = temp;
                    }

                }

            }

            return bubbleList;
        }
    }

    class Bubble
    {
        public string Word;
        public int Count;

        public Bubble(string word, int count)
        {
            this.Word = word;
            this.Count = count;
        }
    }
}