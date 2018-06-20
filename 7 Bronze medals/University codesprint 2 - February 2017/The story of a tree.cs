using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheStoryOfTree
{
    class TheStoryOfTree
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            int numberOfNodesInTheTree = 4;
            var undirectedEdges = new List<Tuple<int, int>>();

            undirectedEdges.Add(new Tuple<int, int>(1, 2));
            undirectedEdges.Add(new Tuple<int, int>(1, 3));
            undirectedEdges.Add(new Tuple<int, int>(3, 4));

            int numberOfGuesses = 2;
            int minimumScoreToWin = 2;

            var parentAndChildPairsGuessed = new List<Tuple<int, int>>();
            parentAndChildPairsGuessed.Add(new Tuple<int, int>(1, 2));
            parentAndChildPairsGuessed.Add(new Tuple<int, int>(3, 4));

            var result = CalculateProbability(
                 numberOfNodesInTheTree,
                 undirectedEdges,
                 numberOfGuesses,
                 minimumScoreToWin,
                 parentAndChildPairsGuessed);
        }

        public static void ProcessInput()
        {
            int queries = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < queries; i++)
            {
                int numberOfNodesInTheTree = Convert.ToInt32(Console.ReadLine());
                var undirectedEdges = new List<Tuple<int, int>>();
                for (int j = 0; j < numberOfNodesInTheTree - 1; j++)
                {
                    var pair = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                    undirectedEdges.Add(new Tuple<int, int>(pair[0], pair[1]));
                }

                var data = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                int numberOfGuesses = data[0];
                int minimumScoreToWin = data[1];

                var parentAndChildPairsGuessed = new List<Tuple<int, int>>();
                for (int j = 0; j < numberOfGuesses; j++)
                {
                    var pair = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                    parentAndChildPairsGuessed.Add(new Tuple<int, int>(pair[0], pair[1]));
                }

                Console.WriteLine(CalculateProbability(
                    numberOfNodesInTheTree,
                    undirectedEdges,
                    numberOfGuesses,
                    minimumScoreToWin,
                    parentAndChildPairsGuessed));
            }
        }

        /*
         * Make the simple as possible           
         */
        public static string CalculateProbability(
                    int numberOfNodesInTheTree,
                    IList<Tuple<int, int>> undirectedEdges,
                    int numberOfGuesses,
                    int minimumScoreToWin,
                    IList<Tuple<int, int>> parentAndChildPairsGuessed
            )
        {
            // timeout issues - how to handle it?
            var guessesGraph = ConvertToGraph(parentAndChildPairsGuessed, false);

            var undirectedEdgesGraph = BuildAGraph(undirectedEdges);
            var numberOfCandidatesForRoot = CountWorkingCandidatesForRoot(numberOfGuesses,
                numberOfNodesInTheTree, minimumScoreToWin, parentAndChildPairsGuessed, undirectedEdgesGraph);

            return reduceFractionInFormatPSlashQ(numberOfCandidatesForRoot, numberOfNodesInTheTree);
        }

        /*
         * timeout concern here, google research about prime number
         */
        private static string reduceFractionInFormatPSlashQ(int p, int q)
        {
            if (p == 0)
            {
                return "0/1";
            }

            int m = q / p;
            if (q % p == 0)
            {
                return 1 + "/" + m;
            }

            int gcd = findGreastCommonDivisor(p, q);
            return p / gcd + "/" + q / gcd;
        }

        /*
         * assuming that p < q
         * https://www.programiz.com/c-programming/examples/hcf-gcd
         */
        private static int findGreastCommonDivisor(int n1, int n2)
        {
            while (n1 != n2)
            {
                if (n1 > n2)
                {
                    n1 -= n2;
                }
                else
                {
                    n2 -= n1;
                }
            }

            return n1;
        }

        /*
         * code review 3:22am 
         */
        private static IDictionary<int, HashSet<int>> BuildAGraph(IList<Tuple<int, int>> edges)
        {
            var graph = new Dictionary<int, HashSet<int>>();

            foreach (var edge in edges)
            {
                var item1 = edge.Item1;
                var item2 = edge.Item2;

                int[] twoItems = new int[] { item1, item2 };

                int count = 0;
                foreach (var item in twoItems)
                {
                    int neighbor = (count == 0) ? item2 : item1;

                    if (graph.ContainsKey(item))
                    {
                        var neighbors = graph[item];
                        neighbors.Add(neighbor);
                        graph[item] = neighbors;
                    }
                    else
                    {
                        var neighbors = new HashSet<int>();
                        neighbors.Add(neighbor);
                        graph.Add(item, neighbors);
                    }

                    count++;
                }
            }

            return graph;
        }

        private static IDictionary<int, HashSet<int>> ConvertToGraph(
            IList<Tuple<int, int>> edges, bool idSmallFirst = false)
        {
            var graph = new Dictionary<int, HashSet<int>>();

            foreach (var edge in edges)
            {
                var item1 = edge.Item1;
                var item2 = edge.Item2;

                if (idSmallFirst && item1 > item2)
                {
                    // always item1 < item2
                    swap(ref item1, ref item2);
                }

                if (graph.ContainsKey(item1))
                {
                    var neighbors = graph[item1];
                    neighbors.Add(item2);
                    graph[item1] = neighbors;
                }
                else
                {
                    var neighbors = new HashSet<int>();
                    neighbors.Add(item2);
                    graph.Add(item1, neighbors);
                }
            }

            return graph;
        }

        private static void swap(ref int item1, ref int item2)
        {
            int tmp = item1;
            item1 = item2;
            item2 = tmp;
        }

        private static int CountWorkingCandidatesForRoot(
            int numberOfGuess,
            int numberOfNodes,
            int minimumScoreToWin,
            IList<Tuple<int, int>> guessesPassChecking,
            IDictionary<int, HashSet<int>> undirectedEdgesGraph)
        {
            var guessesGraph = ConvertToGraph(guessesPassChecking);
            int rootCandidates = 0;


            // try to expedite the search - go through the nodes with more neighbors first
            var highProbableNodesFirst = new HashSet<int>();

            foreach (var node in guessesGraph.OrderByDescending(x => x.Value.Count))
            {
                highProbableNodesFirst.Add(node.Key);
            }

            for (int i = 1; i <= numberOfNodes; i++)
            {
                highProbableNodesFirst.Add(i);
            }

            // try to exclue some nodes - last try 
            int excludeChecking = numberOfGuess - minimumScoreToWin;

            var childCount = getChildCount(guessesGraph);

            foreach (var item in childCount.Where(x => x.Value > excludeChecking))
            {
                highProbableNodesFirst.Remove(item.Key);
            }

            foreach (var id in highProbableNodesFirst)
            {
                int count = 0;
                // Use BFS search starting from root node, then count how many are in the guesses
                Queue<int> queue = new Queue<int>();
                queue.Enqueue(id);

                var nodeVisited = new HashSet<int>();

                while (queue.Count > 0)
                {
                    int visited = queue.Dequeue();
                    nodeVisited.Add(visited);

                    var children = undirectedEdgesGraph[visited];
                    foreach (var child in children)
                    {
                        if (nodeVisited.Contains(child))
                        {
                            continue;
                        }

                        queue.Enqueue(child);

                        if (!guessesGraph.ContainsKey(visited))
                        {
                            continue;
                        }

                        var neighbors = guessesGraph[visited];
                        if (neighbors.Contains(child))
                        {
                            count++;

                            if (count >= minimumScoreToWin)
                            {
                                break; // foreach                                  
                            }
                        }
                    }

                    if (count >= minimumScoreToWin)
                    {
                        break;
                    }
                }

                if (count >= minimumScoreToWin)
                {
                    rootCandidates++;
                }
            }

            return rootCandidates;
        }

        private static IDictionary<int, int> getChildCount(IDictionary<int, HashSet<int>> graph)
        {
            var countMap = new Dictionary<int, int>();

            foreach (var id in graph.Keys)
            {
                var items = graph[id];

                foreach (var item in items)
                {
                    if (countMap.ContainsKey(item))
                    {
                        countMap[item]++;
                    }
                    else
                    {
                        countMap.Add(item, 1);
                    }
                }
            }

            return countMap;
        }

        private static IList<Tuple<int, int>> CountHowManyGuessesAreInUndirectedEdges(
            IList<Tuple<int, int>> parentAndChildPairsGuess,
            IDictionary<int, HashSet<int>> undirectedEdgesMap)
        {
            var selected = new List<Tuple<int, int>>();

            foreach (var pair in parentAndChildPairsGuess)
            {
                var parent = pair.Item1;
                var child = pair.Item2;

                var item1 = Math.Min(parent, child);
                var item2 = Math.Max(parent, child);

                if (undirectedEdgesMap.ContainsKey(item1))
                {
                    var connected = undirectedEdgesMap[item1];
                    if (connected.Contains(item2))
                    {
                        selected.Add(pair);
                    }
                }
            }

            return selected;
        }
    }
}
