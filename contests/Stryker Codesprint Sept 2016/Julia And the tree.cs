using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuliaAndSearchTree
{
    class Node
    {
        public int val;
        public Node left;
        public Node right;

        public Node(int v)
        {
            val = v;
            left = null;
            right = null;
        }
    }

    class Program
    {
        /*
         * 
         * Julia and the search tree
         * problem statement:
         * https://www.hackerrank.com/contests/stryker-codesprint/challenges/julia-and-bst
         * 
         * 10:25pm - 10:47pm 
         * read problem statement
         * Too many nodes in the tree
         * 1. Need to use iterative solution/ not recursive/ avoid stack overflow
         * 2. First construct the tree
         * 3. Then, maybe, add the calculation of sum
         * 
         * Submit on 11:23pm 
         * Score 80 out of 80 
         * Cannot believe that it is difficult level! 
         * 
         */
        static void Main(string[] args)
        {
            int n = Convert.ToInt32(Console.ReadLine());

            string[] arr = Console.ReadLine().Split(' ');

            Console.WriteLine(processBST(arr, n));
        }

        /*
         * 10:53pm - start to write code
         * 11:14pm - finish to construct the binary search tree 
         *   Need to think about timout? 
         *   stack overflow? 
         *   what else
         *   Need to add more tasks 
         */
        public static string processBST(string[] arr, int n)
        {
            if (n == 0 || arr == null || arr.Length == 0)
                return "0";

            Node root = new Node(Convert.ToInt32(arr[0]));

            int index = 1;
            int sum = 0;
            while (index < n)
            {
                int val = Convert.ToInt32(arr[index]);
                Node runner = root;

                int level = 0;
                while (true)
                {
                    if (val < runner.val)
                    {
                        if (runner.left != null)
                        {
                            runner = runner.left;
                            level++;
                        }
                        else
                        {
                            runner.left = new Node(val);
                            sum += level + 1;
                            break;
                        }
                    }
                    else if (val > runner.val)
                    {
                        if (runner.right != null)
                        {
                            runner = runner.right;
                            level++;
                        }
                        else
                        {
                            runner.right = new Node(val);
                            sum += level + 1;
                            break;
                        }
                    }
                }

                index++;
            }

            return sum.ToString();
        }

        public static string processBSTPrototype(string[] arr, int n)
        {
            if (n == 0 || arr == null || arr.Length == 0)
                return "0";

            Node root = new Node(Convert.ToInt32(arr[0]));

            int index = 1;
            while (index < n)
            {
                int val = Convert.ToInt32(arr[index]);
                Node runner = root;

                while (true)
                {
                    if (val < runner.val)
                    {
                        if (runner.left != null)
                            runner = runner.left;
                        else
                        {
                            runner.left = new Node(val);
                            break;
                        }
                    }
                    else if (val > runner.val)
                    {
                        if (runner.right != null)
                            runner = runner.right;
                        else
                        {
                            runner.right = new Node(val);
                            break;
                        }
                    }
                }

                index++;
            }

            return string.Empty;
        }
    }
}
