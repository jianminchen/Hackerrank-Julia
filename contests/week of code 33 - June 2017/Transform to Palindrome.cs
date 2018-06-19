using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leetcode516_longestPalindromicSubsequence
{
    /// <summary>
    /// Leetcode 516: longest palindromic subsequence 
    /// https://leetcode.com/problems/longest-palindromic-subsequence/#/description
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
        }

        public void RunTestcaseLongestPalindromicSeq()
        {
            var test = "abbccdbda";
            int length = longestPalindromeSubseq(test);
        }

        public static void ProcessInput()
        {
            var tokens_n = Console.ReadLine().Split(' ');

            int n = Convert.ToInt32(tokens_n[0]);
            int k = Convert.ToInt32(tokens_n[1]);
            int m = Convert.ToInt32(tokens_n[2]);

            var edges = new int[k][];

            for (int i = 0; i < k; i++)
            {
                edges[i] = new int[2];
                string[] tokens_x = Console.ReadLine().Split(' ');

                edges[i][0] = Convert.ToInt32(tokens_x[0]);
                edges[i][1] = Convert.ToInt32(tokens_x[1]);
            }

            var numbersInRow = Console.ReadLine().Split(' ');
            int[] numbers = Array.ConvertAll(numbersInRow, Int32.Parse);

            var length = longestPalindromeSubseqByTransformation(numbers, edges);

            Console.WriteLine(length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static int longestPalindromeSubseqByTransformation(int[] numbers, int[][] edges)
        {
            // union find algorithm - preprocess transformation
            var unionFind = new UnionFind();

            foreach (var edge in edges)
            {
                var left = edge[0];
                var right = edge[1];

                if (unionFind.IsSameGroup(left, right))
                {
                    continue;
                }

                unionFind.Unite(left, right);
            }

            // longest palindromic subsequence algorithm 
            int length = numbers.Length;

            var subsequence = new int[length][];
            for (int i = 0; i < length; i++)
            {
                subsequence[i] = new int[length];
            }

            // assuming i < j, subsequence from index i to index j
            for (int i = length - 1; i >= 0; i--)
            {
                subsequence[i][i] = 1;

                for (int j = i + 1; j < length; j++)
                {
                    var current = numbers[i];
                    var previous = numbers[j];

                    if (unionFind.IsSameGroup(current, previous))
                    {
                        subsequence[i][j] = subsequence[i + 1][j - 1] + 2;
                    }
                    else
                    {
                        subsequence[i][j] = Math.Max(subsequence[i + 1][j], subsequence[i][j - 1]);
                    }
                }
            }

            return subsequence[0][length - 1];
        }

        public static int longestPalindromeSubseq(String s)
        {
            if (s == null || s.Length == 0)
            {
                return 0;
            }

            int length = s.Length;

            var subsequence = new int[length][];
            for (int i = 0; i < length; i++)
            {
                subsequence[i] = new int[length];
            }

            // assuming i < j, subsequence from index i to index j
            for (int i = length - 1; i >= 0; i--)
            {
                subsequence[i][i] = 1;

                for (int j = i + 1; j < length; j++)
                {
                    if (s[i] == s[j])
                    {
                        subsequence[i][j] = subsequence[i + 1][j - 1] + 2;
                    }
                    else
                    {
                        subsequence[i][j] = Math.Max(subsequence[i + 1][j], subsequence[i][j - 1]);
                    }
                }
            }

            return subsequence[0][length - 1];
        }
    }

    public class UnionFind
    {
        private class Node
        {
            public Node Parent { get; set; }  // parent -> root node of disjoint set
            public int Rank { get; set; }
            public int Id { get; set; }

            public HashSet<Node> Children;

            public Node(int id)
            {
                Id = id;
                Children = new HashSet<Node>();
            }
        }

        private Dictionary<object, Node> dictionary = new Dictionary<object, Node>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Node Root(object data)
        {
            if (!dictionary.ContainsKey(data))
            {
                var node = new Node((int)data);
                dictionary.Add(data, node);
                return node;
            }
            else
            {
                var node = dictionary[data];
                return FindRootSetParent(node);
            }
        }

        /// <summary>
        /// find the root node, and also set node's parent node to root node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node FindRootSetParent(Node node)
        {
            if (node.Parent == null)
            {
                return node;
            }

            return node.Parent = FindRootSetParent(node.Parent);
        }

        /// <summary>
        /// 1. Connect two disjoint sets, for two input arguments, 
        /// find those two root nodes of disjoint set, and then merge them
        /// Design:
        /// If two nodes have same rank, put the parent node as x argument 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Unite(IComparable x, IComparable y)
        {
            var xRoot = Root(x);
            var yRoot = Root(y);

            if (xRoot == yRoot)
            {
                return;
            }

            if (xRoot.Rank < yRoot.Rank)
            {
                ChangeParent(yRoot, xRoot);
            }
            else
            {
                ChangeParent(xRoot, yRoot);

                if (xRoot.Rank == yRoot.Rank)
                {
                    xRoot.Rank++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childRoot"></param>
        void ChangeParent(Node parent, Node childRoot)
        {
            if (childRoot.Parent != null)
            {
                childRoot.Parent.Children.Remove(childRoot);
            }

            childRoot.Parent = parent;
            childRoot.Parent.Children.Add(childRoot);

            foreach (var child in childRoot.Children.ToList())
            {
                ChangeParent(parent, child);
            }
        }

        /// <summary>
        /// IsSameGroup
        /// Also it sets two nodes in the dictionary 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsSameGroup(IComparable x, IComparable y)
        {
            return Root(x) == Root(y);
        }

        /// <summary>
        /// code review: May 6 2017
        /// </summary>
        /// <returns></returns>
        public List<long> GetGroups()
        {
            var groups = new List<long>();

            foreach (var group in dictionary.Values.Where(d => d.Parent == null))
            {
                groups.Add(group.Children.Count());
            }

            return groups;
        }
    }
}
