using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxScore
{
    /// <summary>
    /// score 3.50 out of 35
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int n = 3;

            long[] a = new long[] { 4, 8, 5 };

            var used = new HashSet<int>();
            IList<int> sequence = new List<int>();
            long sum = 0;
            Dictionary<string, long> calculated = new Dictionary<string, long>();
            long maxScore = GetMaxScore(a, used, sum, 0, sequence, calculated);
            Console.WriteLine(maxScore);
        }

        public static void ProcessInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string[] a_temp = Console.ReadLine().Split(' ');
            long[] a = Array.ConvertAll(a_temp, Int64.Parse);

            var used = new HashSet<int>();
            IList<int> sequence = new List<int>();
            long sum = 0;
            Dictionary<string, long> calculated = new Dictionary<string, long>();
            long maxScore = GetMaxScore(a, used, sum, 0, sequence, calculated);
            Console.WriteLine(maxScore);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static long GetMaxScore(long[] array, HashSet<int> used, long sum, long score, IList<int> sequence, Dictionary<string, long> calculated)
        {
            if (used.Count == array.Length)
            {
                return 0;
            }

            long maxScore = long.MinValue;
            for (int i = 0; i < array.Length; i++)
            {
                if (used.Contains(i)) continue;

                long current = array[i];
                var currentScore = sum % current;
                var newCopy = new HashSet<int>(used);
                newCopy.Add(i);

                var newSequence = new List<int>(sequence);
                newSequence.Add(i);
                var key = getKey(newSequence);
                if (calculated.ContainsKey(key))
                {
                    currentScore += calculated[key];
                }
                else
                {
                    long value = GetMaxScore(array, newCopy, sum + current, score + currentScore, newSequence, calculated);
                    currentScore += value;
                    calculated.Add(key, value);
                }

                maxScore = (currentScore > maxScore) ? currentScore : maxScore;
            }

            return maxScore;
        }

        private static string getKey(IList<int> sequence)
        {
            string key = "";
            int index = 0;
            foreach (var item in sequence)
            {
                if (index > 0) key += ",";

                key += item;
                index++;
            }

            return key;
        }
    }
}
