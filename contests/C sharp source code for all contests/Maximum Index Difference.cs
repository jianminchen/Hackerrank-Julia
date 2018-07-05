using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimumIndexDifference
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine().ToString());

            string[] arr1 = Console.ReadLine().ToString().Split(' ');
            string[] arr2 = Console.ReadLine().ToString().Split(' ');

            Console.WriteLine(findMinIndex(n, arr1, arr2));
        }

        public static int findMinIndex(int n, string[] arr1, string[] arr2)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();

            int count = 0;
            foreach (string s in arr2)
            {
                data.Add(s, count);
                count++;
            }

            int min = Int32.MaxValue;
            int diff = Int32.MaxValue; // index difference

            int index = 0;
            foreach (string s in arr1)
            {
                int index2 = data[s];
                int newD = Math.Abs(index2 - index);
                int newV = Convert.ToInt32(s);

                if ((newD < diff) || (newD == diff && newV < min))
                {
                    diff = newD;
                    min = newV;
                }

                index++;
            }

            return min;
        }
    }
}
