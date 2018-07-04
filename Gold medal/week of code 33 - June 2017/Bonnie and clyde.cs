using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackerrank_weekOfCode33_bonnieAndClyde
{
    /// <summary>  
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
            int n = 4;
            int m = 4;
            int q = 1;

            var edges = new List<int[]>();

            edges.Add(new int[] { 1, 2 });
            edges.Add(new int[] { 2, 3 });
            edges.Add(new int[] { 2, 4 });
            edges.Add(new int[] { 3, 1 });

            var queries = new List<int[]>();

            queries.Add(new int[] { 2, 4, 1 });

            var result = connectWithRestaurant(edges, queries);

            foreach (var item in result)
            {
                if (item)
                {
                    Console.WriteLine("YES");
                }
                else
                {
                    Console.WriteLine("NO");
                }
            }
        }

        public static void ProcessInput()
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int n = Convert.ToInt32(tokens_n[0]);
            int m = Convert.ToInt32(tokens_n[1]);
            int q = Convert.ToInt32(tokens_n[2]);

            var edges = new List<int[]>();

            for (int index = 0; index < m; index++)
            {
                string[] tokens_u = Console.ReadLine().Split(' ');

                int u = Convert.ToInt32(tokens_u[0]);
                int v = Convert.ToInt32(tokens_u[1]);
                edges.Add(new int[] { u, v });
            }

            var queries = new List<int[]>();

            for (int index = 0; index < q; index++)
            {
                string[] tokens_u = Console.ReadLine().Split(' ');

                int u = Convert.ToInt32(tokens_u[0]);
                int v = Convert.ToInt32(tokens_u[1]);
                int w = Convert.ToInt32(tokens_u[2]);

                queries.Add(new int[] { u, v, w });
            }

            var result = connectWithRestaurant(edges, queries);

            foreach (var item in result)
            {
                if (item)
                {
                    Console.WriteLine("YES");
                }
                else
                {
                    Console.WriteLine("NO");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static List<bool> connectWithRestaurant(List<int[]> edges, List<int[]> queries)
        {
            // union find algorithm - preprocess transformation
            var unionFind = new UnionFind();
            var lookup = new Dictionary<int, HashSet<int>>();

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

            var connecting = new List<bool>();

            foreach (var query in queries)
            {
                var source1 = query[0];
                var source2 = query[1];
                var destination = query[2];

                if (unionFind.IsSameGroup(source1, source2) &&
                   unionFind.IsSameGroup(source1, destination))
                {
                    connecting.Add(testIsolation(query, unionFind, edges));
                }
                else
                {
                    connecting.Add(false);
                }
            }

            return connecting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lookup"></param>
        private static void addToDictionary(int key, int value, Dictionary<int, HashSet<int>> lookup)
        {
            if (lookup.ContainsKey(key))
            {
                var set = lookup[key];
                set.Add(value);
                lookup[key] = set;
            }
            else
            {
                var set = new HashSet<int>();
                set.Add(value);
                lookup.Add(key, set);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="unionFind"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        private static bool testIsolation(
            int[] query,
            UnionFind unionFind,
            List<int[]> edges)
        {
            int source1 = query[0];
            int source2 = query[1];
            int destination = query[2];

            var tested = new HashSet<int>();

            int index = 0;
            while (index < 2)
            {
                var unionFindChild = new UnionFind();

                var excluded = (index == 0) ? source1 : source2;

                foreach (var edge in edges)
                {
                    var left = edge[0];
                    var right = edge[1];

                    bool isNotInGroup = !unionFind.IsSameGroup(left, destination);
                    bool isExcluded = isContainingTheSource(edge, excluded);

                    if (isExcluded || isNotInGroup)
                    {
                        continue;
                    }

                    // work on child unionFindChild
                    if (unionFindChild.IsSameGroup(left, right))
                    {
                        continue;
                    }

                    unionFindChild.Unite(left, right);
                }

                bool source1Excluded = !unionFindChild.IsSameGroup(source1, destination);
                bool source2Excluded = !unionFindChild.IsSameGroup(source2, destination);
                bool foundException = source1Excluded && source2Excluded;

                if (foundException)
                {
                    return false;
                }

                index++;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="excluded"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        private static bool checkExcluded(int[] edge, int excluded, int destination)
        {
            int vertex1 = edge[0];
            int vertex2 = edge[1];
            return (vertex1 == excluded && vertex2 == destination) ||
                   (vertex2 == excluded && vertex1 == destination);
        }

        /// <summary>
        /// code review 
        /// after failing sample test case, the technique is found. 
        /// Remove one of sources, see the other source will be in the group of destination or not. 
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="excluded"></param>
        /// <returns></returns>
        private static bool isContainingTheSource(int[] edge, int excluded)
        {
            return (edge[0] == excluded || edge[1] == excluded);
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
