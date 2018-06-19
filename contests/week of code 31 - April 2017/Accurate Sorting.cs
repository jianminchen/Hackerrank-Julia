using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuratingSorting
{
    class AccuratingSorting
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < n; i++)
            {
                int size = Convert.ToInt32(Console.ReadLine());
                int[] numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                Console.WriteLine(SwapMakeAscendingOrder(numbers) ? "Yes" : "No");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static bool SwapMakeAscendingOrder(int[] numbers)
        {
            int length = numbers.Length;
            if (length == 1)
            {
                return true;
            }

            int previous = numbers[0];
            for (int i = 1; i < length; i++)
            {
                int current = numbers[i];
                int previousValue = i - 1;

                if (previous == previousValue)
                {
                    previous = current;
                    continue;
                }

                if (current != previousValue)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
