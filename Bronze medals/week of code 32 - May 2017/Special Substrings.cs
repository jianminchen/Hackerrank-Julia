using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace specialSubstrings
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcesInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int[] result = GetSpecialSubstrings_timeSmart("bccbbbbc");
        }

        public static void ProcesInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            string s = Console.ReadLine();
            int[] result = GetSpecialSubstrings_timeSmart(s);

            Console.WriteLine(String.Join("\n", result));
        }

        /// <summary>
        /// try to solve timeout issue 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int[] GetSpecialSubstrings_timeSmart(string s)
        {
            int length = s.Length;
            var numbers = new int[length];

            var palindromes = new HashSet<string>();
            var set = new HashSet<string>();

            // preprocess 26 English chars 
            var preprocessed = getPreprocessed(s);

            var lookupCandidates = new HashSet<string>();

            int index = 0;
            for (int i = 0; i < length; i++)
            {
                var lastChar = s[i];
                bool needToAdd = !palindromes.Contains(lastChar.ToString());

                palindromes.Add(lastChar.ToString());

                if (needToAdd)
                {
                    findAllPrefixes(ref set, lastChar.ToString());
                }

                //for(int start = 0; start < i; start++)
                foreach (var start in preprocessed[lastChar - 'a'])
                {
                    if (start >= i)
                    {
                        break;
                    }

                    var candidate = s.Substring(start, i - start + 1);
                    if (lookupCandidates.Contains(candidate)) // also it is one of keys's prefix
                    //if (lookupCandidates.Contains(candidate) || isOneOfKeysPrefix(lookupCandidates, candidate))
                    {
                        continue;
                    }
                    else
                    {
                        lookupCandidates.Add(candidate);
                    }

                    int searchLength = candidate.Length - 2;
                    var between = candidate.Substring(1, searchLength);

                    // if string is empty or is palindrome then add to HashSet. 
                    if (between.Length == 0 ||
                        palindromes.Contains(between))
                    {
                        palindromes.Add(candidate);

                        // increment to add prefix                                                
                        addPrefixIncludingLastchar(ref set, candidate);
                    }
                }

                numbers[index] = set.Count;
                index++;
            }

            return numbers;
        }

        private static bool isOneOfKeysPrefix(HashSet<string> set, string search)
        {
            bool found = false;
            foreach (var item in set)
            {
                if (item.StartsWith(search))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
        private static IList<int>[] getPreprocessed(string s)
        {
            const int SIZE = 26;
            var preprocessed = new List<int>[SIZE];

            for (int i = 0; i < SIZE; i++)
            {
                preprocessed[i] = new List<int>();
            }

            for (int i = 0; i < s.Length; i++)
            {
                var current = s[i];
                preprocessed[current - 'a'].Add(i);
            }

            return preprocessed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <param name="s"></param>
        private static void addPrefixIncludingLastchar(ref HashSet<string> set, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                var current = s.Substring(0, i + 1);
                set.Add(current);
            }

            return;
        }

        private static void findAllPrefixes(ref HashSet<string> set, string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                var current = s.Substring(0, i + 1);
                set.Add(current);
            }

            return;
        }
    }
}
