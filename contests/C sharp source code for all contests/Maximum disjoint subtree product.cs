using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    /// <summary>
    /// https://www.hackerrank.com/contests/world-codesprint-10/challenges/maximum-disjoint-subtree-product
    /// </summary>
    public class Graph
    {
        private Dictionary<long, Node> nodes { set; get; }
        private Dictionary<long, Edge> edges { set; get; }

        // Key = edge number with sign + or -
        private Dictionary<long, long> maxTree { set; get; }
        private Dictionary<long, long> minTree { set; get; }
        private Dictionary<long, long> maxTreeInclusive { set; get; }
        private Dictionary<long, long> minTreeInclusive { set; get; }

        public Graph()
        {
            nodes = new Dictionary<long, Node>();
            edges = new Dictionary<long, Edge>();

            maxTree = new Dictionary<long, long>();
            minTree = new Dictionary<long, long>();
            maxTreeInclusive = new Dictionary<long, long>();
            minTreeInclusive = new Dictionary<long, long>();
        }

        /// <summary>
        /// Node class - ID, ConnectedEdges, weight, using LinkedList for ConnectedEdges
        /// kind of smart using LinkedList for ConnectedEdges 
        /// How to define connected edges for each node? 
        /// Go over the sample test case to figure out, node 1 - weight -9, connected edges
        /// is a linked list with 2 nodes, where first node is edge 1 which are connected by ID 1 to ID 6, 
        /// and the second node is edge 5 which are connected by ID 2 to ID 2. 
        /// </summary>
        internal class Node
        {
            public long Id { get; private set; }
            public long Weight { get; private set; }
            public LinkedList<Edge> ConnectedEdges;

            public Node(long id, long weight)
            {
                Id = id;
                Weight = weight;
                ConnectedEdges = new LinkedList<Edge>();
            }
        }

        internal class Edge
        {
            public long ID { get; private set; }
            public long Node1 { get; private set; }
            public long Node2 { get; private set; }

            public Edge(long id, long node1, long node2)
            {
                ID = id;
                Node1 = node1;
                Node2 = node2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edgeID"></param>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        public void AddEdge(long edgeID, long id1, long id2)
        {
            var edge = new Edge(edgeID, id1, id2);
            edges.Add(edgeID, edge);

            var node1 = nodes[id1];
            var node2 = nodes[id2];

            // each node has connected edges stored as a linked list
            node1.ConnectedEdges.AddLast(edge);
            node2.ConnectedEdges.AddLast(edge);
        }

        public void AddNode(long id, long weight)
        {
            nodes.Add(id, new Node(id, weight));
        }

        /// <summary>
        /// minimum maximum value tree - 
        /// </summary>
        private void fillMinMaxTrees()
        {
            foreach (var edge in edges.Values)
            {
                var id = edge.ID;

                if (!maxTree.ContainsKey(id))
                {
                    fillMinMaxTreeForEdge_DFS(id);
                }

                if (!maxTree.ContainsKey(-id))
                {
                    fillMinMaxTreeForEdge_DFS(-id);
                }
            }
        }

        /// <summary>
        /// how to understand the max tree for the edge?
        /// Take the sample test case, and understand it. 
        /// It is a DFS search, for each edge, try to find max/ min value. 
        /// First edge with edgeID = 1, ID 6 - ID 1, so node 6 only has one extra connected edge to 
        /// node 3, which only has one connected edge.  
        /// Node Id = 1, which has connected edge 5(1-2), then DFS search to edge 4(5-2), then DFS
        /// search edge 2 (4-5), node 4 has only one connected edge, easy to compute weight. 
        /// </summary>
        /// <param name="edgeIDWithSign"></param>
        private void fillMinMaxTreeForEdge_DFS(long edgeIDWithSign)
        {
            long edgeId = Math.Abs(edgeIDWithSign);
            var edge = edges[edgeId];
            var maxId = Math.Max(edge.Node1, edge.Node2);
            var minId = Math.Min(edge.Node1, edge.Node2);

            long nodeId = edgeIDWithSign > 0 ? maxId : minId;
            var node = nodes[nodeId];
            var connectedEdges = node.ConnectedEdges;

            // base case - node with one connected edge. 
            if (connectedEdges.Count == 1)
            {
                var weight = node.Weight;

                maxTree.Add(edgeIDWithSign, weight);
                minTree.Add(edgeIDWithSign, weight);

                maxTreeInclusive.Add(edgeIDWithSign, weight);
                minTreeInclusive.Add(edgeIDWithSign, weight);

                return;
            }

            long max = long.MinValue;
            long min = long.MaxValue;

            long maxInclusive = node.Weight;
            long minInclusive = node.Weight;

            foreach (var item in connectedEdges)
            {
                var current = item.ID;
                var node1 = item.Node1;
                var node2 = item.Node2;

                if (current == edgeId)
                {
                    continue;
                }

                long id = node1 == nodeId ? node2 : node1;
                long signedId = id > nodeId ? current : -current;

                if (!maxTree.ContainsKey(signedId))
                {
                    fillMinMaxTreeForEdge_DFS(signedId);
                }

                max = Math.Max(maxTree[signedId], max);
                min = Math.Min(minTree[signedId], min);

                maxInclusive = Math.Max(maxTreeInclusive[signedId] + maxInclusive, maxInclusive);
                minInclusive = Math.Min(minTreeInclusive[signedId] + minInclusive, minInclusive);
            }

            max = Math.Max(max, maxInclusive);
            min = Math.Min(min, minInclusive);

            maxTree.Add(edgeIDWithSign, max);
            minTree.Add(edgeIDWithSign, min);

            maxTreeInclusive.Add(edgeIDWithSign, maxInclusive);
            minTreeInclusive.Add(edgeIDWithSign, minInclusive);
        }

        /// <summary>
        /// Need to find two disjoint set with maximum value of product of sum 
        /// </summary>
        /// <returns></returns>
        public long FindMaxProduct()
        {
            fillMinMaxTrees();

            long maxProduct = long.MinValue;
            foreach (var edge in edges.Values)
            {
                maxProduct = Math.Max(maxTree[edge.ID] * maxTree[-edge.ID], maxProduct);
                maxProduct = Math.Max(minTree[edge.ID] * minTree[-edge.ID], maxProduct);
            }

            return maxProduct;
        }
    }

    static void Main(String[] args)
    {
        //ProcessInput();
        RunTestcase();
    }

    public static void RunTestcase()
    {
        int n = 6;

        int[] weights = new int[] { -9, -6, -1, 9, -2, 0 };

        int[][] edgeInfo = new int[5][];

        edgeInfo[0] = new int[] { 6, 1 };
        edgeInfo[1] = new int[] { 4, 5 };
        edgeInfo[2] = new int[] { 6, 3 };
        edgeInfo[3] = new int[] { 5, 2 };
        edgeInfo[4] = new int[] { 1, 2 };

        Graph graph = new Graph();
        for (long i = 0; i < n; i++)
        {
            graph.AddNode(i + 1, weights[i]);
        }

        for (int i = 0; i < n - 1; i++)
        {
            graph.AddEdge(i + 1, edgeInfo[i][0], edgeInfo[i][1]);
        }

        Console.WriteLine(graph.FindMaxProduct());
    }

    public static void ProcessInput()
    {
        long n = Convert.ToInt32(Console.ReadLine());

        // The respective weights of each node:
        string[] w_temp = Console.ReadLine().Split(' ');
        int[] w = Array.ConvertAll(w_temp, Int32.Parse);

        Graph graph = new Graph();
        for (long i = 0; i < n; i++)
        {
            graph.AddNode(i + 1, w[i]);
        }

        for (long i = 0; i < n - 1; i++)
        {
            // Node IDs 'u' and 'v' are connected by an edge:
            string[] tokens_n = Console.ReadLine().Split(' ');
            long u = Convert.ToInt32(tokens_n[0]);
            long v = Convert.ToInt32(tokens_n[1]);

            graph.AddEdge(i + 1, u, v);
        }

        Console.WriteLine(graph.FindMaxProduct());
    }
}