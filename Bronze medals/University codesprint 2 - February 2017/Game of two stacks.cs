using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfTwoStacks
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunATestcase(); 
        }

        public static void RunATestcase()
        {
            long[] data = new long[] { 5, 4, 10 };
            long[] stackA = new long[] { 4, 2, 4, 6, 1 };
            long[] stackB = new long[] { 2, 1, 8, 5 };

            PlayGameForMaximumTotalNumbers(data, stackA, stackB);
        }

        public static void ProcessInput()
        {
            long games = Convert.ToInt64(Console.ReadLine());

            for (int i = 0; i < games; i++)
            {
                long[] data = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
                long[] stackA = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);
                long[] stackB = Array.ConvertAll(Console.ReadLine().Split(' '), long.Parse);

                Console.WriteLine(PlayGameForMaximumTotalNumbers(data, stackA, stackB));
            }
        }

        /*
         * This is a simple game, 
         * two stacks - one stack called A, one stack called B
         * start from tracking variable sumOfInteger = 0, upperBound variable 
         * peek two stacks and see if the smaller one will be ok to take, compared to sumOfInteger
         * and then take it if it is in. 
         */
        public static long PlayGameForMaximumTotalNumbers(long[] data, long[] stackA, long[] stackB)
        {
            long numbersOfInteger = 0;

            long indexA = 0;
            long indexB = 0;
            long sumOfSelections = 0;

            long upperBound = data[2];

            long lengthA = data[0];
            long lengthB = data[1];

            while (sumOfSelections < upperBound &&
                  (indexA < lengthA ||
                   indexB < lengthB))
            {
                long maxValue = long.MaxValue;
                long valueA = (indexA < lengthA) ? stackA[indexA] : maxValue;
                long valueB = (indexB < lengthB) ? stackB[indexB] : maxValue;

                long smallerOne = Math.Min(valueA, valueB);

                if (maxValue - sumOfSelections < smallerOne) // avoid overflow
                {
                    break;
                }

                long nextSum = smallerOne + sumOfSelections;

                if (nextSum > upperBound)
                {
                    break;
                }

                if (valueA == smallerOne)
                {
                    indexA++;
                }
                else
                {
                    indexB++;
                }

                numbersOfInteger++;
                sumOfSelections += smallerOne;
            }

            return numbersOfInteger;
        }
    }
}
