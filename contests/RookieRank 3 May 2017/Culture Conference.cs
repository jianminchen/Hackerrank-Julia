#if DEBUG
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    class Program
    {


        static void Main(string[] args)
        {
            ProcessInput();
            //RunUnionFind(); 
            //RunUnionFindTestCase();
        }

        public static void ProcessInput()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            // Information for employees 1 through n - 1:
            // The first value is the employee's supervisor ID
            // The second value is the employee's burnout status (0 is burned out, 1 is not)
            int[][] e = new int[n - 1][];
            for (int e_i = 0; e_i < n - 1; e_i++)
            {
                string[] e_temp = Console.ReadLine().Split(' ');
                e[e_i] = Array.ConvertAll(e_temp, Int32.Parse);
            }

            long minimumEmployees = RunUnionFind(e);

            Console.WriteLine(minimumEmployees);
        }

        /*
         * Calculate the value of friendships
         */
        public static long RunUnionFind(int[][] edges)
        {
            var unionFind = new UnionFind();

            int count = 0;
            int index = 1;
            bool ceoFound = false;
            foreach (var edge in edges)
            {
                var left = index;
                var right = edge[0];
                var burnout = edge[1];

                if (burnout == 1)
                {
                    continue;
                }

                if (right == 0)
                {
                    ceoFound = true;
                }

                if (unionFind.IsSameGroup(left, right))
                {
                    count++;
                    continue;
                }

                unionFind.Unite(right, left);

                index++;
            }
            var bigThanOne = 0;
            var groups = unionFind.GetGroupsSpecial(ref bigThanOne).Where(v => v != 0).Select(v => v + 1).ToList();

            var original = groups.Sum();
            return (ceoFound) ? (original - 1) : original;
        }

        /*
         * Calculate the value of friendships
         */
        public static void RunUnionFindTestCase()
        {
            var unionFind = new UnionFind();
            var edges = new int[4][];
            edges[0] = new int[] { 0, 0 };
            edges[1] = new int[] { 1, 0 };
            edges[2] = new int[] { 2, 0 };
            edges[3] = new int[] { 2, 0 };


            int count = 0;
            int index = 1;
            foreach (var edge in edges)
            {
                var left = index;
                var right = edge[0];
                var burnout = edge[1];


                if (burnout == 1)
                {
                    continue;
                }

                if (unionFind.IsSameGroup(left, right))
                {
                    count++;
                    continue;
                }

                unionFind.Unite(left, right);

                index++;
            }

            var bigThanOne = 0;
            var groups = unionFind.GetGroupsSpecial(ref bigThanOne).Where(v => v != 0).Select(v => v + 1).ToList();

            groups.Sort();
            var result = groups.Sum() - bigThanOne;
        }
    }

    /*
     * Class UnionFind 
     * 
     */
    public class UnionFind
    {
        private class Node
        {
            public Node Parent { get; set; }
            public int Rank { get; set; }
            public int Id { get; set; }

            public HashSet<Node> Children;

            public Node()
            {
                Children = new HashSet<Node>();
            }
        }

        private Dictionary<object, Node> _dict = new Dictionary<object, Node>();

        private Node Root(object data)
        {
            if (!_dict.ContainsKey(data))
            {
                var node = new Node();
                _dict.Add(data, node);
                return node;
            }
            else
            {
                var node = _dict[data];
                return Find(node);
            }
        }

        private Node Find(Node node)
        {
            if (node.Parent == null)
            {
                return node;
            }

            return node.Parent = Find(node.Parent);
        }

        public void Unite(IComparable x, IComparable y)
        {
            var xRoot = Root(x);
            var yRoot = Root(y);
            if (xRoot == yRoot) return;

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
        /// 
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

            foreach (var group in _dict.Values.Where(d => d.Parent == null))
            {
                groups.Add(group.Children.Count());
            }

            return groups;
        }

        public List<long> GetGroupsSpecial(ref int bigThanOne)
        {
            var groups = new List<long>();

            int index = 0;
            foreach (var group in _dict.Values.Where(d => d.Parent == null))
            {
                var number = group.Children.Count();
                if (number == 1)
                {
                    groups.Add(1);
                }
                else
                {
                    int count = saveParentChildren(group);
                    groups.Add(number - count);
                    index++;
                }
            }

            return groups;
        }

        private int saveParentChildren(Node node)
        {
            if (node == null || node.Children.Count == 0) return 0;

            var number = node.Children.Count;
            int count = node.Children.Count - 1;

            if (number == 1) count = 1;
            foreach (var child in node.Children)
            {
                count += saveParentChildren(child);
            }

            return count;
        }
    }

#if DEBUG
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void Tests()
        {
           /*
            var n = 5;
            var m = 4;
            var friends = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(1, 2),
                new Tuple<int, int>(3, 2),
                new Tuple<int, int>(4, 2),
                new Tuple<int, int>(4, 3),
            };

            Assert.AreEqual(32, Program.CalculateValueOfFriendships(n, friends));          
            * */
        }       
    }

#endif
}