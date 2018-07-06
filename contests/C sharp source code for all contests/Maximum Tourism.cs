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
        }

        public static void ProcessInput()
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int m = Convert.ToInt32(tokens_n[1]);
            int[][] route = new int[m][];
            for (int route_i = 0; route_i < m; route_i++)
            {
                string[] route_temp = Console.ReadLine().Split(' ');
                route[route_i] = Array.ConvertAll(route_temp, Int32.Parse);
            }

            Console.WriteLine(RunUnionFind(route));
        }

        /*
         * Calculate the value of friendships
         */
        public static long RunUnionFind(int[][] edges)
        {
            var unionFind = new UnionFind();

            int count = 0;
            foreach (var edge in edges)
            {
                var left = edge[0];
                var right = edge[1];
                if (left == right)
                {
                    continue;
                }

                if (unionFind.IsSameGroup(left, right))
                {
                    count++;
                    continue;
                }

                unionFind.Unite(left, right);
            }

            var groups = unionFind.GetGroups().Where(v => v != 0).Select(v => v + 1).ToList();

            groups.Sort();
            groups.Reverse();

            return groups[0];
        }

        /*
         * Calculate the value of friendships
         */
        public static void RunUnionFindTestCase()
        {
            var unionFind = new UnionFind();
            var edges = new int[6][];
            edges[0] = new int[] { 1, 2 };
            edges[1] = new int[] { 7, 4 };
            edges[2] = new int[] { 7, 3 };
            edges[3] = new int[] { 5, 8 };
            edges[4] = new int[] { 1, 3 };
            edges[5] = new int[] { 7, 3 };

            int count = 0;
            foreach (var edge in edges)
            {
                var left = edge[0];
                var right = edge[1];
                if (left == right)
                {
                    continue;
                }

                if (unionFind.IsSameGroup(left, right))
                {
                    count++;
                    continue;
                }

                unionFind.Unite(left, right);
            }

            var groups = unionFind.GetGroups().Where(v => v != 0).Select(v => v + 1).ToList();

            groups.Sort();
            groups.Reverse();
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

        /*
         * write something here...
         */
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

        public bool IsSameGroup(IComparable x, IComparable y)
        {
            return Root(x) == Root(y);
        }

        public List<long> GetGroups()
        {
            var groups = new List<long>();

            foreach (var group in _dict.Values.Where(d => d.Parent == null))
            {
                groups.Add(group.Children.Count());
            }

            return groups;
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