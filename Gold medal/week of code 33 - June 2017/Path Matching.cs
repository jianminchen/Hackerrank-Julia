using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatchMatching
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

    class PatchMatching
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int n = 5;
            int queries = 5;
            string symbols = "abbaa";
            string pattern = "ab";

            var edges = new List<int[]>();

            edges.Add(new int[] { 1, 2 });
            edges.Add(new int[] { 2, 4 });
            edges.Add(new int[] { 2, 5 });
            edges.Add(new int[] { 1, 3 });
            edges.Add(new int[] { 4, 5 });

            var queryEdges = new List<int[]>();

            queryEdges.Add(new int[] { 5, 4 });
            queryEdges.Add(new int[] { 4, 3 });
            queryEdges.Add(new int[] { 3, 4 });
            queryEdges.Add(new int[] { 2, 4 });

            var result = PathMatching(edges, queryEdges, symbols, pattern);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void ProcessInput()
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int queries = Convert.ToInt32(tokens_n[1]);
            string symbols = Console.ReadLine();
            string pattern = Console.ReadLine();

            var edges = new List<int[]>();
            for (int i = 0; i < n - 1; i++)
            {
                string[] tokens_u = Console.ReadLine().Split(' ');
                int u = Convert.ToInt32(tokens_u[0]);
                int v = Convert.ToInt32(tokens_u[1]);

                edges.Add(new int[] { u, v });
            }

            var queryEdges = new List<int[]>();

            for (int i = 0; i < queries; i++)
            {
                string[] tokens_u = Console.ReadLine().Split(' ');
                int u = Convert.ToInt32(tokens_u[0]);
                int v = Convert.ToInt32(tokens_u[1]);

                queryEdges.Add(new int[] { u, v });
            }

            var result = PathMatching(edges, queryEdges, symbols, pattern);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// Julia, you have to make it simple as possible, solve the problem, score anything first. 
        /// June 18, 2017, 8:07 pm 
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="queriesEdges"></param>
        /// <returns></returns>
        public static List<int> PathMatching(
            List<int[]> edges,
            List<int[]> queriesEdges,
            string symbol,
            string pattern)
        {
            // put into quick access place
            var memo = new Dictionary<int, HashSet<int>>();

            foreach (var edge in edges)
            {
                var left = edge[0];
                var right = edge[1];

                addToDictionary(memo, left, right);
                addToDictionary(memo, right, left);
            }

            var findPatterns = new List<int>();
            //
            foreach (var edge in queriesEdges)
            {
                var start = edge[0];
                var end = edge[1];
                var visited = new HashSet<int>();

                var path = string.Empty;
                var result = searchPath(start, end, memo, visited, ref path);

                if (!result)
                {
                    findPatterns.Add(0);
                }
                else
                {
                    // need to convert to a string 
                    findPatterns.Add(searchPattern(symbol, pattern, path));
                }
            }

            return findPatterns;
        }

        /// <summary>
        /// Need to find pattern quickly using KMP, rabin-karp etc. 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="pattern"></param>
        /// <param name="path"></param>
        private static int searchPattern(string symbol, string pattern, string path)
        {
            var pathString = convert(symbol, path);

            var stringSearch = new BoyerMoore(pattern);

            int count = 0;
            while (pathString != null &&
                  pathString.Length >= pattern.Length)
            {
                int offset = stringSearch.search(pathString);

                if (offset < pathString.Length)
                {
                    count++;

                    pathString = pathString.Substring(offset + 1);
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        private static string convert(string symbol, string path)
        {
            var builder = new StringBuilder();
            var numbers = Array.ConvertAll(path.Split(' '), int.Parse);
            foreach (var item in numbers)
            {
                builder.Append(symbol[item - 1]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// depth first search to find the route using recursive function 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="end"></param>
        /// <param name="memo"></param>
        /// <param name="visited"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool searchPath(
            int current,
            int end,
            Dictionary<int, HashSet<int>> memo,
            HashSet<int> visited,
            ref string path)
        {
            // base case 
            if (current == end)
            {
                path += (path.Length > 0) ? " " : "";
                path += end;
                return true;
            }

            // visited node - exit
            if (visited.Contains(current))
            {
                return false;
            }

            // mark the visit
            visited.Add(current);

            path += (path.Length > 0) ? " " : "";
            path += current;

            var neighbors = memo[current];

            foreach (var neighbor in neighbors)
            {
                var backtracking = new HashSet<int>(visited);
                var backupPath = path;

                var result = searchPath(neighbor, end, memo, visited, ref path);
                if (result)
                {
                    return true;
                }

                visited = backtracking;
                path = backupPath;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memo"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void addToDictionary(Dictionary<int, HashSet<int>> memo, int key, int value)
        {
            if (memo.ContainsKey(key))
            {
                var set = memo[key];
                set.Add(value);

                memo[key] = set;
            }
            else
            {
                var set = new HashSet<int>();
                set.Add(value);

                memo.Add(key, set);
            }
        }
    }
}
