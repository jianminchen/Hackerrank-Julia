#if DEBUG
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace JuliaPracticeOnApril42017
{
    /// <summary>
    /// Study the article: 
    /// http://www.geeksforgeeks.org/greedy-algorithms-set-2-kruskals-minimum-spanning-tree-mst/
    /// 
    /// greedy algorithm set 2 - kurskals minimum spanning tree 
    /// April 14, 2017 10:00 am - 1:00 pm 
    /// </summary>

    class Graph
    {
        // A class to represent a graph edge
        internal class Edge
        {
            public int src { get; set; }
            public int dest { get; set; }
            public int weight { get; set; }
            public int antecedent { get; set; }
            public int consequent { get; set; }
            public bool isCycle { get; set; }
            public bool isMinimum { get; set; }

            // Comparator function used for sorting edges based on
            // their weight - only score 8 out 40
            public static int Compare_Naive(Edge a, Edge b)
            {
                double first = 1.0 * a.consequent / a.antecedent;
                double second = 1.0 * b.consequent / b.antecedent;
                if (first > second)
                {
                    return 1;
                }
                else if (Math.Abs(first - second) < 0.001) // 1 - 100, 1/100 = 0.01
                {
                    return a.antecedent - b.antecedent;
                }
                else
                {
                    return -1;
                }
            }
            /// <summary>
            /// maximum -> minimum problem 
            /// a/b maximum -> b/a minimum 
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static int Compare(Edge a, Edge b)
            {
                int first = a.consequent * b.antecedent;
                int second = b.consequent * a.antecedent;

                if (first == second)
                {
                    return a.antecedent - b.antecedent;
                }
                else
                {
                    return first - second;
                }
            }
        };

        // A class to represent a subset for union-find
        internal class Subset
        {
            public int parent { get; set; }
            public int rank { get; set; }
        };

        // V-> no. of vertices & E->no.of edges
        public int noVertices, noEdges;

        // collection of all edges
        Edge[] edges;

        // Creates a graph with V vertices and E edges
        public Graph(int v, int e)
        {
            noVertices = v;
            noEdges = e;
            edges = new Edge[noEdges];

            for (int i = 0; i < e; ++i)
            {
                edges[i] = new Edge();
            }
        }

        /// <summary>
        /// A utility function to find set of an element i
        /// (uses path compression technique)
        /// </summary>
        /// <param name="subsets"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        int find(Subset[] subsets, int i)
        {
            // find root and make root as parent of i (path compression)
            if (subsets[i].parent != i)
            {
                subsets[i].parent = find(subsets, subsets[i].parent);
            }

            return subsets[i].parent;
        }

        // A function that does union of two sets of x and y
        // (uses union by rank)
        public void Union(Subset[] subsets, int x, int y)
        {
            int xroot = find(subsets, x);
            int yroot = find(subsets, y);

            // Attach smaller rank tree under root of high rank tree
            // (Union by Rank)
            if (subsets[xroot].rank < subsets[yroot].rank)
            {
                subsets[xroot].parent = yroot;
            }
            else if (subsets[xroot].rank > subsets[yroot].rank)
            {
                subsets[yroot].parent = xroot;
            }

            // If ranks are same, then make one as root and increment
            // its rank by one
            else
            {
                subsets[yroot].parent = xroot;
                subsets[xroot].rank++;
            }
        }

        /// <summary>
        /// Construct minimum spanning tree using Kruskal's algorithm
        /// </summary>
        public Edge[] KruskalMST()
        {
            int mstEdges = noVertices - 1;
            var minimumSpanTree = new Edge[mstEdges];

            for (int i = 0; i < mstEdges; ++i)
            {
                minimumSpanTree[i] = new Edge();
            }

            // sort by weight 
            Array.Sort(edges, Edge.Compare);

            // Allocate memory for creating subsets
            var subsets = new Subset[noVertices];

            for (int i = 0; i < noVertices; ++i)
            {
                subsets[i] = new Subset();
            }

            // Create subsets with single elements
            for (int v = 0; v < noVertices; ++v)
            {
                subsets[v].parent = v;
                subsets[v].rank = 0;
            }

            int edgeIndex = 0;  // Index used to pick next edge

            int treeIndex = 0;

            // Number of edges to be taken is equal to V-1
            while (treeIndex < noVertices - 1)
            {
                // Step 2: Pick the smallest edge. And increment the index
                // for next iteration
                var edge = new Edge();

                edge = edges[edgeIndex++];

                int rootOfSource = find(subsets, edge.src);
                int rootOfDestination = find(subsets, edge.dest);

                // If including this edge does't cause cycle, include it
                // in result and increment the index of result for next edge
                if (rootOfSource != rootOfDestination)
                {
                    minimumSpanTree[treeIndex++] = edge;
                    Union(subsets, rootOfSource, rootOfDestination);
                }
                // Else discard the next_edge
            }

            return minimumSpanTree;
        }

        internal class Node
        {
            public int Start { get; set; }
            public int End { get; set; }

            public string Key
            {
                get
                {
                    return Start + "," + End;
                }
            }

            public string AlternateKey
            {
                get
                {
                    return End + "," + Start;
                }
            }

            public int Antecedent { get; set; }
            public int Consequent { get; set; }

            public Node(int start, int end, int ante, int conse)
            {
                Start = start;
                End = end;
                Antecedent = ante;
                Consequent = conse;
            }
        }

        static void Main(string[] args)
        {
            ProcesInput();
            //RunSampleTestcase(); 
        }

        public static void RunSampleTestcase()
        {
            int vertices = 3;
            int edges = 3;


            var row1 = new int[] { 0, 1, 1, 1 };
            var row2 = new int[] { 1, 2, 2, 4 };
            var row3 = new int[] { 2, 0, 1, 2 };

            IDictionary<string, Node> lookup = new Dictionary<string, Node>();

            for (int i = 0; i < edges; i++)
            {
                var numbers = row1;
                if (i == 1) numbers = row2;
                if (i == 2) numbers = row3;

                int start = numbers[0];
                int end = numbers[1];
                int antecedent = numbers[2];
                int consequent = numbers[3];
                var ratio = 1.0 * consequent / antecedent;

                var node = new Node(start, end, antecedent, consequent);
                if (start == end)
                {
                    continue; // skip the loop
                }

                var key = node.Key;
                var alternativeKey = node.AlternateKey;
                if (lookup.ContainsKey(key))
                {
                    // only keep the small one, consequent/ antecedent
                    var current = lookup[key];
                    var currentRatio = 1.0 * current.Consequent / current.Antecedent;

                    if (ratio < currentRatio)
                    {
                        lookup[key] = node;
                    }
                }
                else if (lookup.ContainsKey(alternativeKey))
                {
                    var current = lookup[alternativeKey];
                    var currentRatio = 1.0 * current.Antecedent / current.Consequent;

                    if (ratio < currentRatio)
                    {
                        lookup[key] = node;
                    }
                }
                else
                {
                    lookup.Add(key, node);
                }
            }

            var result = CallKrusalAlgorithm(lookup, vertices, edges);
        }

        public static void ProcesInput()
        {
            var input = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
            int noVertices = input[0];
            int noEdges = input[1];

            IDictionary<string, Node> lookup = new Dictionary<string, Node>();

            for (int i = 0; i < noEdges; i++)
            {
                var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                int start = numbers[0];
                int end = numbers[1];
                int antecedent = numbers[2];
                int consequent = numbers[3];

                var node = new Node(start, end, antecedent, consequent);
                if (start == end)
                {
                    continue; // skip the loop
                }

                var key = node.Key;
                var alternativeKey = node.AlternateKey;
                if (lookup.ContainsKey(key))
                {
                    // only keep the small one, consequent/ antecedent
                    Node current = lookup[key];
                    int oldAntecedent = current.Antecedent;
                    int oldConsequent = current.Consequent;

                    int currentWeight = consequent * oldAntecedent;
                    int oldWeight = antecedent * oldConsequent;

                    // replace the old one 
                    if (currentWeight < oldWeight ||
                        (currentWeight == oldWeight &&
                         consequent < oldConsequent))
                    {
                        lookup[key] = node;
                    }
                }
                // remove the branch else if, the test result is the same, no impact! 
                else if (lookup.ContainsKey(alternativeKey))
                {
                    // only keep the small one, consequent/ antecedent
                    Node current = lookup[alternativeKey];
                    int oldAntecedent = current.Antecedent;
                    int oldConsequent = current.Consequent;

                    int currentWeight = consequent * oldAntecedent;
                    int oldWeight = antecedent * oldConsequent;

                    if (currentWeight < oldWeight ||
                         (currentWeight == oldWeight && consequent < oldConsequent)
                       )
                    {
                        lookup.Remove(alternativeKey);
                        lookup[key] = node;
                    }
                }
                else
                {
                    lookup.Add(key, node);
                }
            }

            var result = CallKrusalAlgorithm(lookup, noVertices, noEdges);

            Console.WriteLine(result);
        }

        /// <summary>
        ///  Start = start;
        ///        End = end;
        ///        Antecedent = ante;
        ///        Consequent = conse; 
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="vertices"></param>
        /// <param name="edges"></param>
        public static string CallKrusalAlgorithm(IDictionary<string, Node> lookup, int vertices, int edges)
        {
            var graph = new Graph(vertices, edges);

            int index = 0;
            foreach (var keyValuePair in lookup)
            {
                var node = keyValuePair.Value;

                graph.edges[index].src = node.Start;
                graph.edges[index].dest = node.End;
                graph.edges[index].antecedent = node.Antecedent;
                graph.edges[index].consequent = node.Consequent;

                index++;
            }

            var minimumSpanningTree = graph.KruskalMST();

            int sumTop = 0;
            int sumBottom = 0;

            for (int i = 0; i < minimumSpanningTree.Length; i++)
            {
                var edge = minimumSpanningTree[i];
                int top = edge.antecedent;
                int bottom = edge.consequent;

                sumTop += top;
                sumBottom += bottom;
            }

            return GetReductionForm(sumTop, sumBottom);
        }

        public static string GetReductionForm(int top, int bottom)
        {
            int greatCommonDivisor = GreatestCommonDivisor(top, bottom);
            return top / greatCommonDivisor + "/" + bottom / greatCommonDivisor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            if (b == 0) return a;
            return GreatestCommonDivisor(b, a % b);
        }
    }
}