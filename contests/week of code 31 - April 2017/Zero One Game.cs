using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zeroOneGame
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
            IList<int> numbers = new List<int> { 1, 0, 1, 0, 1 };
            var result = CountMoves(numbers, 0);
        }

        public static void ProcessInput()
        {
            int queries = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < queries; i++)
            {
                int length = Convert.ToInt32(Console.ReadLine());
                var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                IList<int> numbersEasyToRemove = new List<int>(numbers);

                var result = CountMoves(numbersEasyToRemove, 0);

                if (result % 2 == 0)
                {
                    Console.WriteLine("Bob");
                }
                else
                {
                    Console.WriteLine("Alice");
                }
            }
        }

        /// <summary>
        /// just use recursive solution to solve the problem
        /// solve timeout issue - avoid duplicated checking as many as possible 
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static int CountMoves(IList<int> numbers, int start)
        {
            int length = numbers.Count;
            if (length <= 2)
            {
                return 0;
            }

            int actualStart = Math.Max(start, 0);
            int previous = numbers[actualStart];

            for (int i = actualStart + 1; i + 1 < length; i++)
            {
                int current = numbers[i];
                int next = numbers[i + 1];

                // if current is removable 
                if (previous == 0 && next == 0)
                {
                    numbers.RemoveAt(i);
                    return 1 + CountMoves(numbers, i - 2);
                }

                // move to next iteration 
                previous = current;
            }

            return 0;
        }
    }
}