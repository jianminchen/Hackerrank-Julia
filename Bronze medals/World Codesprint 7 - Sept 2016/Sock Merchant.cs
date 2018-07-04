using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SockMerchant
{
    class Program
    {
        /*
         * Sept. 24, 026
         * 
         * start: 1:21pm 
         * Time complexity: O(n)
         * Space complexity: O(n)
         * exit: 1:26pm
         * start: 1:33pm, wrong answer, run time error
         * array size 100 -> 101
         * exit: 1:34pm 
         */
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string[] arr = Console.ReadLine().Split(' ');

            Console.WriteLine(calculateNo(n, arr));

        }

        /*start: 1:27pm
         * exit: 1:31pm
         */
        public static int calculateNo(int n, string[] arr)
        {
            int SIZE = 101;
            int[] colors = new int[SIZE];

            for (int i = 0; i < arr.Length; i++)
            {
                int val = Convert.ToInt32(arr[i]);
                colors[val]++;
            }

            int sum = 0;
            for (int i = 0; i < SIZE; i++)
            {
                sum += colors[i] / 2;
            }

            return sum;
        }
    }
}
