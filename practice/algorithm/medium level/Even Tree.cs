using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTreeGraphAlgorithm
{
    /// <summary>
    /// June 22, 2018
    /// code review
    /// https://www.hackerrank.com/challenges/even-tree/problem
    /// 
    /// </summary>
    class Graph
    {
        private int V;
        private List<Tuple<int, int>>[] edges;
        private Dictionary<int, List<int>> vertIndex;

        public Graph(int V)
        {
            this.V = V;
            edges = new List<Tuple<int, int>>[V + 1];

            vertIndex = new Dictionary<int, List<int>>();
            for (int i = 0; i < V + 1; i++)
            {
                // bug001 - index starting from 1, not 0, so array size V+1
                edges[i] = new List<Tuple<int, int>>();
                vertIndex[i] = new List<int>();
            }
        }

        public void addEdge(int v, int w)
        {
            edges[v].Add(new Tuple<int, int>(v, w));
            vertIndex[v].Add(w);
            vertIndex[w].Add(v);
        }

        public int countNoRemovableEdges()
        {
            int count = 0;
            for (int i = 1; i <= V; i++)  // include V, vertices are numbered from 1, 2, ..., V
            {
                List<Tuple<int, int>> list = edges[i];

                for (int j = 0; j < list.Count; j++)
                {
                    HashSet<int> touched = new HashSet<int>();
                    Queue<int> queue = new Queue<int>();

                    int cur = list[j].Item1;
                    int next = list[j].Item2;

                    touched.Add(i);
                    touched.Add(next);

                    queue.Enqueue(next);
                    while (queue.Count > 0)
                    {
                        int first = queue.Dequeue();

                        foreach (int connect in vertIndex[first])
                        {
                            if (!touched.Contains(connect))
                            {
                                touched.Add(connect);
                                queue.Enqueue(connect);
                            }
                        }
                    }

                    if ((touched.Count % 2) == 1)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] s = Console.ReadLine().Split(' ');
            int noVertices = Convert.ToInt32(s[0]);
            int noEdges = Convert.ToInt32(s[1]);

            Graph graph = new Graph(noVertices);
            for (int i = 0; i < noEdges; i++)
            {
                string[] s1 = Console.ReadLine().Split(' ');

                int start = Convert.ToInt32(s1[0]);
                int end = Convert.ToInt32(s1[1]);

                graph.addEdge(start, end);
            }

            Console.WriteLine(graph.countNoRemovableEdges());
        }
    }
}
