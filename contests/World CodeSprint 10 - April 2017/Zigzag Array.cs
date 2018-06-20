using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zigzagArray
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            var size = 5;

            var numbers = new int[] { 5, 2, 3, 6, 1 };

            var minimumNumbers = CalculateMinimumNumbersToZigZag(size, numbers);

            Console.WriteLine(minimumNumbers);
        }

        public static void ProcessInput()
        {
            var size = Convert.ToInt32(Console.ReadLine());

            var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            var minimumNumbers = CalculateMinimumNumbersToZigZag(size, numbers);

            Console.WriteLine(minimumNumbers);
        }

        /// <summary>
        /// calculate peak numbers and valley numbers, and then total number of array 
        /// minus the sum of peak number and valley numbers
        /// </summary>
        /// <param name="size"></param>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int CalculateMinimumNumbersToZigZag(int size, int[] numbers)
        {
            if (size <= 2)
            {
                return 0;
            }

            int first = numbers[0];

            var peaks = 0;
            var valleys = 0;
            for (int i = 1; i < size - 1; i++)
            {
                int second = numbers[i];
                int third = numbers[i + 1];
                if (isPeak(first, second, third))
                {
                    peaks++;
                }
                else if (isValley(first, second, third))
                {
                    valleys++;
                }

                //  next iteration 
                first = second;
            }

            return size - peaks - valleys - 2;
        }

        private static bool isPeak(int first, int second, int third)
        {
            return second > first && second > third;
        }

        private static bool isValley(int first, int second, int third)
        {
            return second < first && second < third;
        }
    }
}
