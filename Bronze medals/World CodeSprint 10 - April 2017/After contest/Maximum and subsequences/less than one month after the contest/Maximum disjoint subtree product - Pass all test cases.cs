using System;
using System.Collections.Generic;
using System.IO;
class Solution
{
    /// <summary>
    /// https://www.hackerrank.com/contests/world-codesprint-10/challenges/maximum-disjoint-subtree-product
    /// </summary>
    public class Graph
    {
        private Dictionary<long, Node> nodes { set; get; }
        private Dictionary<long, Edge> edges { set; get; }

        //one edge with one edge id, but use negative edge id 
        //as well to accommodate two nodes in the edge.  
        private Dictionary<long, long> edgesWithMaxWeight { set; get; }
        private Dictionary<long, long> edgesWithMinimumWeight { set; get; }
        private Dictionary<long, long> edgesWithMaxWeightInclusive { set; get; }
        private Dictionary<long, long> edgesWithMinWeightInclusive { set; get; }

        public Graph()
        {
            nodes = new Dictionary<long, Node>();
            edges = new Dictionary<long, Edge>();

            edgesWithMaxWeight = new Dictionary<long, long>();
            edgesWithMinimumWeight = new Dictionary<long, long>();
            edgesWithMaxWeightInclusive = new Dictionary<long, long>();
            edgesWithMinWeightInclusive = new Dictionary<long, long>();
        }

        /// <summary>
        /// Node class - ID, ConnectedEdges, weight, 
        /// using LinkedList for ConnectedEdges
        /// kind of smart using LinkedList for ConnectedEdges 
        /// How to define connected edges for each node? 
        /// Go over the sample test case to figure out, 
        /// node 1 - weight -9, connected edges
        /// is a linked list with 2 nodes, where first node is edge 1 
        /// which are connected by Id 1 and Id 6, 
        /// and the second node is edge 5 which are connected by 
        /// Id 1 and Id 2. 
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

            /// <summary>
            /// one edge has one edge id and two nodes with two node's 
            /// Ids, then the node with large node id
            /// takes the positive edge id whereas the small one 
            /// uses negative edge id.         
            /// </summary>
            /// <param name="signedEdgeId"></param>
            /// <returns></returns>
            public static long GetEdgeId(long signedEdgeId)
            {
                return Math.Abs(signedEdgeId);
            }

            /// <summary>
            /// one edge has two node ids, and then one edge id and 
            /// its negative value both
            /// are used for edge id. 
            /// </summary>
            /// <param name="signedEdgeId"></param>
            /// <returns></returns>
            public long GetNodeId(long signedEdgeId)
            {
                return signedEdgeId > 0 ? GetMaxOne() : GetMinimumOne();
            }

            public long GetMaxOne()
            {
                return Math.Max(Node1, Node2);
            }

            public long GetMinimumOne()
            {
                return Math.Min(Node1, Node2);
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

                if (!edgesWithMaxWeight.ContainsKey(id))
                {
                    fillMinMaxTreeForEdge_DFS(id);
                }

                if (!edgesWithMaxWeight.ContainsKey(-id))
                {
                    fillMinMaxTreeForEdge_DFS(-id);
                }
            }
        }

        /// <summary>
        /// how to understand the max tree for the edge?
        /// Take the sample test case, and understand it. 
        /// It is a DFS search, for each edge, try to find max/ min value. 
        /// First edge with edgeId = 1, node Id 6 connects to node Id 1, 
        /// so node id 6 only has one extra connected edge to node 3,
        ///  which only has one connected edge.  
        /// Node Id = 1, which has connected edge 5(1-2), then DFS 
        /// search to edge 4(5-2), then DFS
        /// search edge 2 (4-5), node 4 has only one connected edge 
        /// which is the base case.  
        /// </summary>
        /// <param name="signedEdgeId"></param>
        private void fillMinMaxTreeForEdge_DFS(long signedEdgeId)
        {
            long edgeId = Edge.GetEdgeId(signedEdgeId);
            var edge = edges[edgeId];
            long nodeId = edge.GetNodeId(signedEdgeId);

            var node = nodes[nodeId];
            var connectedEdges = node.ConnectedEdges;

            // base case - node with one connected edge. 
            if (connectedEdges.Count == 1)
            {
                var weight = node.Weight;

                edgesWithMaxWeight.Add(signedEdgeId, weight);
                edgesWithMinimumWeight.Add(signedEdgeId, weight);

                edgesWithMaxWeightInclusive.Add(signedEdgeId, weight);
                edgesWithMinWeightInclusive.Add(signedEdgeId, weight);

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

                if (!edgesWithMaxWeight.ContainsKey(signedId))
                {
                    fillMinMaxTreeForEdge_DFS(signedId);
                }

                max = Math.Max(edgesWithMaxWeight[signedId], max);
                min = Math.Min(edgesWithMinimumWeight[signedId], min);

                maxInclusive = Math.Max(edgesWithMaxWeightInclusive[signedId] + maxInclusive, maxInclusive);
                minInclusive = Math.Min(edgesWithMinWeightInclusive[signedId] + minInclusive, minInclusive);
            }

            max = Math.Max(max, maxInclusive);
            min = Math.Min(min, minInclusive);

            edgesWithMaxWeight.Add(signedEdgeId, max);
            edgesWithMinimumWeight.Add(signedEdgeId, min);

            edgesWithMaxWeightInclusive.Add(signedEdgeId, maxInclusive);
            edgesWithMinWeightInclusive.Add(signedEdgeId, minInclusive);
        }

        /// <summary>
        /// Need to find two disjoint set with maximum value of 
        /// product of sum. Each disjoint set has
        /// its weight to sum all the nodes's weight. 
        /// </summary>
        /// <returns></returns>
        public long FindMaxProductTwoDisjointSets()
        {
            fillMinMaxTrees();

            long maxProduct = long.MinValue;
            foreach (var edge in edges.Values)
            {
                var id = edge.ID;
                var set1Value = edgesWithMaxWeight[id];
                var set2Value = edgesWithMaxWeight[-id];
                maxProduct = Math.Max(set1Value * set2Value, maxProduct);

                set1Value = edgesWithMinimumWeight[id];
                set2Value = edgesWithMinimumWeight[-id];
                maxProduct = Math.Max(set1Value * set2Value, maxProduct);
            }

            return maxProduct;
        }
    }

    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase(); 
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

        var graph = new Graph();
        for (long i = 0; i < n; i++)
        {
            graph.AddNode(i + 1, weights[i]);
        }

        for (int i = 0; i < n - 1; i++)
        {
            graph.AddEdge(i + 1, edgeInfo[i][0], edgeInfo[i][1]);
        }

        Console.WriteLine(graph.FindMaxProductTwoDisjointSets());
    }

    public static void ProcessInput()
    {
        long n = Convert.ToInt32(Console.ReadLine());

        // The respective weights of each node:
        string[] w_temp = Console.ReadLine().Split(' ');
        int[] w = Array.ConvertAll(w_temp, Int32.Parse);

        var graph = new Graph();
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

        Console.WriteLine(graph.FindMaxProductTwoDisjointSets());
    }
}
