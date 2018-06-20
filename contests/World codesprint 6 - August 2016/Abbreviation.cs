using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abbreviation
{
    class Program
    {
        static void Main(string[] args)
        {
            //test(); 
            hackRank();
        }

        public static void hackRank()
        {
            int queries = Convert.ToInt32(Console.ReadLine().Trim());

            for (int i = 0; i < queries; i++)
            {
                string s1 = Console.ReadLine();
                string s2 = Console.ReadLine();

                Console.WriteLine((CanConvert(s1, s2) ? "YES" : "NO"));
            }
        }
        public static void test()
        {
            string s1 = "daBcd";
            string s2 = "ABC";

            bool res = CanConvert(s1, s2);
        }

        public static bool CanConvert(string s1, string s2)
        {
            int[] lowerArr1 = new int[26];
            int[] upperArr1 = new int[26];
            int[] arr2 = new int[26];

            if (s1 == null || s1.Length == 0)
                return false;

            foreach (char c in s1.Trim())
            {
                int no = c - 'a';
                bool isLower = isLowerCase(no);
                if (isLower)
                    lowerArr1[no]++;
                else
                    upperArr1[c - 'A']++;
            }

            foreach (char c in s2.Trim())
            {
                arr2[c - 'A']++;
            }

            // preprocessing 
            // check s1 and s2 count of char from A to Z
            for (int i = 0; i < 26; i++)
            {
                int total = lowerArr1[i] + upperArr1[i];
                if (total < arr2[i] || upperArr1[i] > arr2[i])
                    return false;
            }

            int index1 = 0;
            int index2 = 0;
            while (index2 < s2.Length)
            {
                if (index1 >= s1.Length)
                    return false;

                char c1 = s1[index1];
                char c2 = s2[index2];

                int no = (c1 - 'a');
                bool isLower = no >= 0 && no < 26;

                if (!isLower)
                    no = c1 - 'A';

                if ((c1 == c2) || (c1 - 'a') == (c2 - 'A'))
                {
                    // if isLower, then discuss skip no is available or not
                    bool convertLowerToUpper = isLower && (arr2[no] > upperArr1[no]);
                    bool bothUpper = c1 == c2;
                    if (bothUpper || convertLowerToUpper)
                    {
                        index2++;
                        arr2[no]--;
                    }
                }

                index1++;
                if (isLower)
                    lowerArr1[no]--;
                else
                    upperArr1[no]--;

                if (!isOk(lowerArr1, upperArr1, arr2, no))
                    return false;
            }

            return true;
        }

        public static bool isLowerCase(int no)
        {
            return no >= 0 && no < 26;
        }

        /*
         * test case: 
         * s1 = "aA", s2="A" - ok
         * s1 = "AA", s2="A" - false
         */
        public static bool isOk(int[] lowerArr1, int[] upperArr1, int[] arr2, int index)
        {
            int total = lowerArr1[index] + upperArr1[index];
            if (total < arr2[index] || upperArr1[index] > arr2[index])
                return false;

            return true;
        }
    }
}
