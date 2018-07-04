using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonAppetit
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arr = Console.ReadLine().Split(' ');
            int len = Convert.ToInt32(arr[0].Trim());
            int index = Convert.ToInt32(arr[1].Trim());

            string[] arr2 = Console.ReadLine().Split(' ');
            int split = Convert.ToInt32(Console.ReadLine().Trim());

            int res = calculateDiff(arr2, len, index, split);
            if (res == 0)
                Console.WriteLine("Bon Appetit");
            else
                Console.WriteLine(res.ToString());

        }

        /*
         * 
         */
        public static int calculateDiff(string[] arr, int len, int index, int split)
        {
            int sum = 2 * split;
            for (int i = 0; i < len; i++)
            {
                int tmp = Convert.ToInt32(arr[i]);
                if (i != index)
                    sum -= tmp;
            }

            return (sum == 0) ? 0 : sum / 2;
        }
    }
}
