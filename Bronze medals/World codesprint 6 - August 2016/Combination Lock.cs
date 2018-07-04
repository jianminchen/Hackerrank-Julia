using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace combinationLock
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arr = Console.ReadLine().Split(' ');
            string[] arr2 = Console.ReadLine().Split(' ');

            Console.WriteLine(calculate(arr, arr2));
        }

        public static int calculate(string[] arr, string[] arr2)
        {
            if ((arr == null || arr.Length == 0) ||
                (arr2 == null || arr2.Length == 0) ||
               (arr.Length != arr2.Length))
                return 0;

            int sum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                int val1 = Convert.ToInt32(arr[i].ToString());
                int val2 = Convert.ToInt32(arr2[i].ToString());

                int diff = Math.Abs(val1 - val2);
                sum += (diff > 5) ? (10 - diff) : diff;
            }

            return sum;
        }
    }
}
