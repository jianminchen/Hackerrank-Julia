using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Solution
{
    /// <summary>
    /// code review
    /// First, I spent time to read editorial notes
    /// https://www.hackerrank.com/contests/world-codesprint-10/challenges/maximum-disjoint-subtree-product/editorial
    /// And then I downloaded one of C# solutions:
    /// https://www.hackerrank.com/rest/contests/world-codesprint-10/challenges/maximum-disjoint-subtree-product/hackers/rpkooter/download_solution
    /// 
    /// </summary>
    /// <param name="args"></param>
    static void Main(String[] args)
    {
        int n = Convert.ToInt32(Console.ReadLine());

        // The respective weights of each node:
        var splitted = Console.ReadLine().Split(' ');
        var weights = Array.ConvertAll(splitted, Int32.Parse);

        var nodes = new Node[n];
        for (int index = 0; index < n; index++)
        {
            nodes[index] = new Node()
            {
                Id = index,
                Weight = weights[index]
            };
        }

        for (int index = 0; index < n - 1; index++)
        {
            // Node IDs 'u' and 'v' are connected by an edge:
            string[] uv_temp = Console.ReadLine().Split(' ');
            int[] uv = Array.ConvertAll(uv_temp, Int32.Parse);

            int u = uv[0] - 1;
            int v = uv[1] - 1;

            // Write Your Code Here
            nodes[u].Children.Add(nodes[v]);
            nodes[v].Children.Add(nodes[u]);
        }

        // Convert
        ConvertToTree(nodes[0]);

        long highest = GetMaximumOrMinimumValue(nodes, weights, false);
        long highest2 = GetMaximumOrMinimumValue(nodes, weights, true);

        long best = Math.Max(highest, highest2);

        Console.WriteLine(best);
    }

    /// <summary>
    /// code review May 30, 2018
    /// The function is to design maximum and minimum value calculation.
    /// For minimum value, set weight to its negative value ? 
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="getMinimum"></param>
    private static long GetMaximumOrMinimumValue(Node[] nodes, int[] weights, bool getMinimum)
    {
        if (getMinimum)
        {
            for (int index = 0; index < nodes.Length; index++)
            {
                nodes[index].Weight = -weights[index];

                nodes[index].InclusiveFromParent = 0;
                nodes[index].InclusiveToParent = 0;

                nodes[index].NonInclusiveFromParent = 0;
                nodes[index].NonInclusiveToParent = 0;
            }
        }

        var rootNode = nodes[0];

        DfsWalk(rootNode);
        ReverseDfsWalk(rootNode);

        return GetHighest(rootNode);
    }

    /// <summary>
    /// code review May 30, 2018
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    private static long GetHighest(Node root)
    {
        long highestProduct = long.MinValue;

        var nodesToWalk = new Queue<Node>();

        nodesToWalk.Enqueue(root);
        while (nodesToWalk.Any())
        {
            var next = nodesToWalk.Dequeue();
            foreach (Node child in next.Children)
            {
                nodesToWalk.Enqueue(child);
            }

            // Process calculate InclusiveFromParent for each child            
            if (next == root)
            {
                continue;
            }

            long highestFrom = Math.Max(next.InclusiveFromParent, next.NonInclusiveFromParent);
            long highestTo = Math.Max(next.InclusiveToParent, next.NonInclusiveToParent);

            long product = highestTo * highestFrom;
            highestProduct = Math.Max(highestProduct, product);
        }

        return highestProduct;
    }

    /// <summary>
    /// code review on May 30, 2018
    /// </summary>
    /// <param name="root"></param>
    private static void ReverseDfsWalk(Node root)
    {
        var nodesToWalk = new Queue<Node>();

        nodesToWalk.Enqueue(root);

        while (nodesToWalk.Any())
        {
            var next = nodesToWalk.Dequeue();

            foreach (Node child in next.Children)
            {
                nodesToWalk.Enqueue(child);
            }

            // Process calculate InclusiveFromParent for each child
            long inclusiveScore = next.Weight;

            if (next.InclusiveFromParent > 0)
            {
                inclusiveScore += next.InclusiveFromParent;
            }

            foreach (Node child in next.Children)
            {
                if (child.InclusiveToParent > 0)
                {
                    inclusiveScore += child.InclusiveToParent;
                }
            }

            foreach (Node child in next.Children)
            {
                child.InclusiveFromParent = inclusiveScore -
                    (child.InclusiveToParent > 0 ? child.InclusiveToParent : 0);
            }

            // Process calculate NonInclusiveFromParent for each child
            var maxScores = new List<long>();

            maxScores.Add(int.MinValue);

            maxScores.Add(Math.Max(next.InclusiveFromParent, next.NonInclusiveFromParent));

            foreach (Node child in next.Children)
            {
                maxScores.Add(Math.Max(child.InclusiveToParent, child.NonInclusiveToParent));
            }

            maxScores = maxScores.OrderBy(x => x).ToList();

            foreach (Node child in next.Children)
            {
                long score = Math.Max(child.InclusiveToParent, child.NonInclusiveToParent);

                child.NonInclusiveFromParent = maxScores[maxScores.Count - 1] != score ?
                    maxScores[maxScores.Count - 1] : maxScores[maxScores.Count - 2];
            }
        }
    }

    /// <summary>
    /// code review on May 30, 2018
    /// </summary>
    /// <param name="root"></param>
    private static void DfsWalk(Node root)
    {
        var nodesToWalk = new Queue<Node>();

        nodesToWalk.Enqueue(root);
        var dfsList = new Stack<Node>();

        while (nodesToWalk.Any())
        {
            var top = nodesToWalk.Dequeue();
            dfsList.Push(top);

            foreach (Node child in top.Children)
            {
                nodesToWalk.Enqueue(child);
            }
        }

        while (dfsList.Any())
        {
            var next = dfsList.Pop();
            if (next != root)
            {
                next.InclusiveToParent = next.Weight;
                foreach (Node child in next.Children)
                {
                    if (child.InclusiveToParent > 0)
                    {
                        next.InclusiveToParent += child.InclusiveToParent;
                    }

                    long score = Math.Max(child.InclusiveToParent, child.NonInclusiveToParent);
                    next.NonInclusiveToParent = Math.Max(next.NonInclusiveToParent, score);
                }
            }
        }
    }

    /// <summary>
    /// code review on May 30, 2018
    /// </summary>
    /// <param name="root"></param>
    private static void ConvertToTree(Node root)
    {
        var nodesToConvert = new Queue<Node>();

        nodesToConvert.Enqueue(root);

        while (nodesToConvert.Any())
        {
            var nextNode = nodesToConvert.Dequeue();

            foreach (Node child in nextNode.Children)
            {
                child.Parent = nextNode;
                child.Children.Remove(nextNode);
                nodesToConvert.Enqueue(child);
            }
        }
    }

    /// <summary>
    /// code review on May 30, 2018
    /// </summary>
    class Node
    {
        public Node Parent;
        public List<Node> Children = new List<Node>();

        public int Id;
        public long Weight;

        // to parent
        public long InclusiveToParent = 0;
        public long NonInclusiveToParent = 0;

        // from parent
        public long InclusiveFromParent = 0;
        public long NonInclusiveFromParent = 0;
    }
}
