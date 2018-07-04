using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawBook
{
    class Program
    {
        static void Main(string[] args)
        {
            TailorShop();
            //RunSampleTestCase1();
        }

        private static void RunSampleTestCase1()
        {
            long sum = CalMinimumSumWithDistinctNoButtons(3, 2, new int[] { 4, 6, 6 });
            Debug.Assert(sum == 9);
        }

        private static void TailorShop()
        {
            int[] data = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            int colors = data[0];
            int cost = data[1];

            int[] minimumCosts = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            Console.WriteLine(CalMinimumSumWithDistinctNoButtons(colors, cost, minimumCosts));
        }
        /*
         * spec: 
         * 1. Distinct number of buttons of a certain color
         * each color 
         */
        private static long CalMinimumSumWithDistinctNoButtons(
            int colors,
            int cost,
            int[] minimumCosts
            )
        {
            Array.Sort(minimumCosts);

            ConvertToNumber(minimumCosts, cost);

            Dictionary<int, int> countingData = new Dictionary<int, int>();

            countingData = Count(minimumCosts);

            int[] keyData = countingData.Keys.ToArray();
            Array.Sort(keyData);

            long sum = 0;
            int needToFill = 0;
            bool isFirst = true;
            int prev = -1;
            foreach (int value in keyData)
            {
                int count = countingData[value];

                // take care some duplicates
                int start = prev + 1;
                int end = value - 1;
                int gap = end - start;

                if (needToFill > 0 &&
                    gap > 0 &&
                    !isFirst)
                {


                    if (needToFill <= gap)
                    {
                        int end2 = start + needToFill - 1;
                        sum += SumValue(start, end2);
                        needToFill = 0;
                    }
                    else
                    {
                        sum += SumValue(prev, value);
                        needToFill -= gap;
                    }
                }

                sum += value;
                prev = value;
                if (count > 1)
                {
                    needToFill += count - 1;
                }

                if (isFirst)
                    isFirst = false;
            }

            //edge case
            if (needToFill > 0)
                sum += SumValue(prev + 1, prev + needToFill);

            // add the sum of numbers
            return sum;
        }

        private static void ConvertToNumber(int[] minimumCosts, int cost)
        {
            for (int i = 0; i < minimumCosts.Length; i++)
            {
                int value = minimumCosts[i];
                int number = value / cost;
                if (number * cost < value)
                    number++;

                minimumCosts[i] = number;
            }
        }

        private static long SumValue(int start, int end)
        {
            long sum = (long)start + (long)end;

            return sum * (end - start + 1) / 2;
        }

        private static Dictionary<int, int> Count(int[] minimumCosts)
        {
            Dictionary<int, int> countingData = new Dictionary<int, int>();

            foreach (int value in minimumCosts)
            {
                if (countingData.ContainsKey(value))
                {
                    countingData[value]++;
                }
                else
                {
                    countingData[value] = 1;
                }
            }

            return countingData;
        }
    }
}
