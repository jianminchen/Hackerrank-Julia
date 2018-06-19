using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fightMonsters
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] tokens_n = Console.ReadLine().Split(' ');

            int n = Convert.ToInt32(tokens_n[0]);
            int hit = Convert.ToInt32(tokens_n[1]);

            int t = Convert.ToInt32(tokens_n[2]);

            var rowOfHits = Console.ReadLine().Split(' ');

            int[] hitNumbers = Array.ConvertAll(rowOfHits, Int32.Parse);
            int result = GetMaxMonsters(n, hit, t, hitNumbers);
            Console.WriteLine(result);
        }

        /// <summary>
        /// Be careful that range of long data type
        /// </summary>
        /// <param name="n"></param>
        /// <param name="hit"></param>
        /// <param name="tSeconds"></param>
        /// <param name="hitNumbers"></param>
        /// <returns></returns>
        public static int GetMaxMonsters(int n, int hit, int tSeconds, int[] hitNumbers)
        {
            Array.Sort(hitNumbers);

            long secondsLeft = tSeconds;
            for (int i = 0; i < n; i++)
            {
                var current = hitNumbers[i];

                bool addOne = current % hit > 0;
                int number = current / hit;

                var secondsConsumed = addOne ? (number + 1) : number;

                if (secondsConsumed <= secondsLeft)
                {
                    secondsLeft -= secondsConsumed;
                    continue;
                }

                return i;
            }

            return n;
        }
    }
}
