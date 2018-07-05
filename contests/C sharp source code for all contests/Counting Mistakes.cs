using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace countingMistakes
{
    class Program
    {
        /*
         * counting mistakes 
         * Nov. 2, 2016
         * 
         */
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            int[] arr = ToInt(Console.ReadLine().Split(' '));

            Console.WriteLine(calculateMistakes(arr));
        }

        /*
         * start: 11:37am
         * exit: 11:40am 
         */
        private static int calculateMistakes(int[] arr)
        {
            int count = 0;
            int len = arr.Length;
            if (arr[0] != 1)
                count++;

            for (int i = 1; i < len; i++)
            {
                if ((arr[i] - arr[i - 1]) != 1)
                    count++;
            }

            return count;
        }

        private static int[] ToInt(string[] arr)
        {
            int len = arr.Length;
            int[] res = new int[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = Convert.ToInt32(arr[i]);
            }

            return res;
        }


    }
}
