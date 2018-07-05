using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheHiddenMessage
{
    public class BoyerMoore
    {
        private int R;     // the radix
        private int[] right;     // the bad-character skip array

        private char[] pattern;  // store the pattern as a character array
        private String pat;      // or as a string

        /**
         * 
         * code source from Java code
         * 
         * http://algs4.cs.princeton.edu/53substring/BoyerMoore.java.html
         * Preprocesses the pattern string.
         *
         * @param pat the pattern string
         */
        public BoyerMoore(String pat)
        {
            this.R = 256;
            this.pat = pat;

            // position of rightmost occurrence of c in the pattern
            right = new int[R];
            for (int c = 0; c < R; c++)
                right[c] = -1;
            for (int j = 0; j < pat.Length; j++)
                right[pat[j]] = j;
        }

        /**
         * Preprocesses the pattern string.
         *
         * @param pattern the pattern string
         * @param R the alphabet size
         */
        public BoyerMoore(char[] pattern, int R)
        {
            this.R = R;
            this.pattern = new char[pattern.Length];
            for (int j = 0; j < pattern.Length; j++)
                this.pattern[j] = pattern[j];

            // position of rightmost occurrence of c in the pattern
            right = new int[R];
            for (int c = 0; c < R; c++)
                right[c] = -1;
            for (int j = 0; j < pattern.Length; j++)
                right[pattern[j]] = j;
        }

        /**
         * Returns the index of the first occurrrence of the pattern string
         * in the text string.
         *
         * @param  txt the text string
         * @return the index of the first occurrence of the pattern string
         *         in the text string; N if no such match
         */
        public int search(String txt)
        {
            int M = pat.Length;
            int N = txt.Length;
            int skip;

            for (int i = 0; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (pat[j] != txt[i + j])
                    {
                        skip = Math.Max(1, j - right[txt[i + j]]);
                        break;
                    }
                }

                if (skip == 0) return i;    // found
            }

            return N;                       // not found
        }


        /**
         * Returns the index of the first occurrrence of the pattern string
         * in the text string.
         *
         * @param  text the text string
         * @return the index of the first occurrence of the pattern string
         *         in the text string; N if no such match
         */
        public int search(char[] text)
        {
            int M = pattern.Length;
            int N = text.Length;
            int skip;
            for (int i = 0; i <= N - M; i += skip)
            {
                skip = 0;
                for (int j = M - 1; j >= 0; j--)
                {
                    if (pattern[j] != text[i + j])
                    {
                        skip = Math.Max(1, j - right[text[i + j]]);
                        break;
                    }
                }
                if (skip == 0) return i;    // found
            }
            return N;                       // not found
        }
    }

    class Match
    {
        public string content;
        public int start;
        public int end;
    }

    /* 7:08pm - start to read the problem statement
     * 
     * 7:47pm start to write down her approach
     * start position is increasing 
     * How to find word match? 
     * 
     * Time complexity - 
     * Data structure 
     * Space complexity: 
     * 
     * 7:55pm start to code
     * 
     * 10:04pm start to conduct testing
     */
    class Program
    {
        /*
         * 8:05pm start to write code
         */
        static void Main(string[] args)
        {
            theHiddenMessage();
            //getCountSubstringUsingBoyerAlgo("cde","abcdefg"); 
        }

        public static void theHiddenMessage()
        {
            string message = Console.ReadLine().ToString().Trim();
            string[] arr = Console.ReadLine().ToString().Trim().Split(' ');

            IList<string> output = processHiddenMessage(message, arr);

            foreach (string s in output)
            {
                Console.WriteLine(s);
            }
        }

        /*
         *  8:05pm start to write code
         *  8:43pm still work on the function -
         *  9:00pm review the function 
         *  - add another function - calculate cost 
         */
        public static IList<string> processHiddenMessage(string message, string[] arr)
        {
            string[] Text = { "YES", "NO", "0" };

            IList<string> res = new List<string>();
            if (arr == null || arr.Length == 0)
            {
                res.Add(Text[1]);
                res.Add(Text[2]);

                return res;
            }

            int start = 0;
            int index = 0;
            int len = message.Length;
            int size = arr.Length;

            IList<Match> data = new List<Match>();

            while (start < len && index < size)
            {
                string s1 = message.Substring(start, len - start);
                string s2 = arr[index];

                int pos = 0;
                bool found = findUsingBoyerAlgo(s2, s1, ref pos);

                if (!found)
                {
                    break;
                }

                Match match = new Match();
                match.content = s2;
                match.start = start + pos;
                match.end = start + pos + s2.Length - 1;

                data.Add(match);

                // next iteration
                start += pos + 1;
                index++;
            }

            int val = data.Count;
            if (val == 0)
            {
                res.Add(Text[1]);
                res.Add(Text[2]);
                res.Add(Text[2]);
            }
            else if (val < size)
            {
                res.Add(Text[1]);
                res.Add(toString(data));
                res.Add(Text[2]);
            }
            else if (val == size)
            {
                res.Add(Text[0]);
                res.Add(toString(data));
                res.Add(calculateCost(data, message));
            }

            return res;
        }

        /*
         * 9:02pm - start to code
         * 9:43pm - still work on the calculation of cost
         * - try to think about how many chars to be removed - second step
         * 9:57pm use brute force solution first 
         */
        public static string calculateCost(IList<Match> data,
            string message
            )
        {
            int sum = sumLength(data);

            int len = message.Length;
            int[] arr = new int[message.Length];

            foreach (Match item in data)
            {
                for (int i = item.start; i <= item.end; i++)
                {
                    arr[i] = 1;
                }
            }

            int removed = 0;
            for (int i = 0; i < len; i++)
            {
                if (arr[i] == 0)
                    removed++;
            }

            return (sum - len + 2 * removed).ToString();
        }

        public static int sumLength(IList<Match> data)
        {
            int count = 0;
            foreach (Match item in data)
            {
                count += item.content.Length;
                count++;
            }

            count--;
            return count;
        }

        /*
         * 8:52pm start to code 
         * 8:58pm exit
         */
        private static string toString(IList<Match> data)
        {
            string res = string.Empty;

            foreach (Match item in data)
            {
                res += item.content;
                res += " ";
                res += item.start.ToString();
                res += " ";
                res += item.end.ToString();
                res += " ";
            }

            return res.Substring(0, res.Length - 1);
        }


        /*
         * 8:24pm 
         * copy code from blog:
         * http://juliachencoding.blogspot.ca/2016/04/hackerrank-string-function-calculation_10.html
         * 
         * 8:36 prepare to exit the function
         */
        private static bool findUsingBoyerAlgo(string substring, string s, ref int start)
        {
            int searchLen = substring.Length;
            int len = s.Length;

            if (searchLen > len)
                return false;

            BoyerMoore bm = new BoyerMoore(substring);

            int offset = bm.search(s);
            if (offset < len)
            {
                start = offset;
                return true;
            }

            return false;
        }
    }
}
