using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    internal class Node
    {
        public int Index { get; set; }

        public Node Previous { get; set; }
        public Node Next { get; set; }

        public int Left { get; set; }
        public int Right { get; set; }

        public long ExtraSweet { get; set; }

        // public bool Used { get; set; }   // consider one more time

        public Node(int value)
        {
            Index = value;
        }

        /// <summary>
        /// calculate sweat
        /// </summary>
        public void CalculateSweat()
        {
            ExtraSweet = (long)(Left + Right) * (Right - Left + 1) / 2;
        }

        public void AddExtraLeft(Node[] nodes, int n)
        {
            var leftNode = FindExtraLeft(nodes, n);
            if (leftNode == null)
            {
                return;
            }

            ExtraSweet += leftNode.Index;

            //setLeftNodeLeftNeighbor(leftNode, nodes, n); 
        }

        public void SetRightNeighborPreviousLinkDefault(Node[] nodes, int n)
        {
            var next = getNext(nodes, n);

            if (next == null)
            {
                return;
            }

            // right neighbor's previous to left neighbor
            next.Previous = nodes[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodes"></param>
        /// <param name="n"></param>
        public void SetDoubleLinksForLeftOfExtraLeft(Node[] nodes, int n)
        {
            var node = FindExtraLeft(nodes, n);
            if (node == null)
            {
                return;
            }

            // check node left neighbor 
            // two case: with/ no left neighbor
            var withPrevious = node.Previous != null;
            var current = withPrevious ? node.Previous : (node.Index >= 1 ? nodes[node.Index - 1] : null);

            if (current == null)
            {
                SetRightNeighborPreviousLinkDefault(nodes, n);
                return;
            }

            var next = getNext(nodes, n);

            if (next == null)
            {
                current.Next = nodes[n]; // point it to dummy node  
                return;
            }

            // left neighbor's next to right neighbor
            current.Next = next;

            // right neighbor's previous to left neighbor
            next.Previous = current;
        }

        /// <summary>
        /// work on the testing more!
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private Node getNext(Node[] nodes, int n)
        {
            var rightNode = nodes[Right].FindExtraRight(nodes, n);
            if (rightNode == null)
            {
                return null;
            }

            var withNext = rightNode.Next != null;

            return withNext ? rightNode.Next : (rightNode.Index < n - 1 ? nodes[rightNode.Index + 1] : null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="n"></param>
        public void AddExtraRight(Node[] nodes, int n)
        {
            var rightNode = nodes[Right].FindExtraRight(nodes, n);
            if (rightNode == null)
            {
                return;
            }

            ExtraSweet += rightNode.Index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public Node FindExtraLeft(Node[] nodes, int n)
        {
            var withPrevious = this.Previous != null;

            return withPrevious ? this.Previous : (this.Index > 0 ? nodes[this.Index - 1] : null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public Node FindExtraRight(Node[] nodes, int n)
        {
            var withNext = this.Next != null;

            return withNext ? this.Next : (this.Index < n - 1 ? nodes[this.Index + 1] : null);
        }
    }

    static void Main(String[] args)
    {
        ProcessInput();

        //RunTestcase(); 

    }

    public static void RunTestcase()
    {
        int n = 10;
        int queries = 3;

        var nodes = new Node[n + 1];

        for (int index = 0; index < n; index++)
        {
            nodes[index] = new Node(index);
        }

        nodes[n] = new Node(0); // dummy node with Index 0 value 

        var leftNodes = new int[] { 2, 6, 9 };
        var rightNodes = new int[] { 4, 7, 9 };

        var sweat = new long[queries];

        for (int index = 0; index < queries; index++)
        {
            int l = leftNodes[index];
            int r = rightNodes[index];

            nodes[l].Left = l;
            nodes[l].Right = r;

            nodes[l].CalculateSweat();
            nodes[l].AddExtraLeft(nodes, n);
            nodes[l].AddExtraRight(nodes, n);
            nodes[l].SetDoubleLinksForLeftOfExtraLeft(nodes, n);

            sweat[index] = nodes[l].ExtraSweet;
        }

        for (int i = 0; i < queries; i++)
        {
            Console.WriteLine(sweat[i]);
        }
    }

    public static void ProcessInput()
    {
        string[] tokens_n = Console.ReadLine().Split(' ');
        int n = Convert.ToInt32(tokens_n[0]);
        int queries = Convert.ToInt32(tokens_n[1]);

        var nodes = new Node[n + 1];

        for (int index = 0; index < n; index++)
        {
            nodes[index] = new Node(index);
        }

        nodes[n] = new Node(0); // dummy node with Index 0 value 

        var sweat = new long[queries];

        for (int index = 0; index < queries; index++)
        {
            string[] tokens_l = Console.ReadLine().Split(' ');

            int l = Convert.ToInt32(tokens_l[0]);
            int r = Convert.ToInt32(tokens_l[1]);

            nodes[l].Left = l;
            nodes[l].Right = r;

            nodes[l].CalculateSweat();
            nodes[l].AddExtraLeft(nodes, n);
            nodes[l].AddExtraRight(nodes, n);
            nodes[l].SetDoubleLinksForLeftOfExtraLeft(nodes, n);

            sweat[index] = nodes[l].ExtraSweet;
        }

        for (int i = 0; i < queries; i++)
        {
            Console.WriteLine(sweat[i]);
        }
    }
}
