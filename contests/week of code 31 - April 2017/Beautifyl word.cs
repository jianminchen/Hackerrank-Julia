using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beautifulWord
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();

            if (IsBeautifulWord(s))
            {
                Console.WriteLine("Yes");
            }
            else
            {
                Console.WriteLine("No");
            }
        }

        /// <summary>
        /// 1 <= length(s) <=100
        /// void chars: "aeiouy"
        /// Rule 1: no two consecutive words are the same
        /// Rule 2: no two consecutive words are in the above vowel set.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBeautifulWord(string s)
        {
            int length = s.Length;
            if (s.Length == 1)
            {
                return true;
            }

            var vowels = "aeiouy";

            var previous = s[0];
            var current = previous;

            for (int i = 1; i < length; i++)
            {
                current = s[i];
                if (current == previous ||
                    (vowels.IndexOf(current) >= 0 &&
                    vowels.IndexOf(previous) >= 0))
                {
                    return false;
                }

                previous = current;
            }

            return true;
        }
    }
}
