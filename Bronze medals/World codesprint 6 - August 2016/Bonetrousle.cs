using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boneTrousle
{
    public class Node
    {
        public string purchaseBoxes;
        public long length;
        public int boxCount;
        public long sum;
    }


    class Program
    {
        static void Main(string[] args)
        {
            int queries = Convert.ToInt32(Console.ReadLine().Trim());

            HashSet<string> excluded = new HashSet<string>();
            for (int i = 0; i < queries; i++)
            {
                string[] arr = Console.ReadLine().Split(' ');

                long total = Convert.ToInt64(arr[0]);
                long storeKBox = Convert.ToInt64(arr[1]);
                int purchaseBoxCount = Convert.ToInt32(arr[2]);

                IList<string> list = new List<string>();

                buildResult_usingDFS_stack(total, storeKBox, purchaseBoxCount, list, excluded);  // think about design here

                string res = "-1";

                if (list.Count > 0)
                {
                    string[] arr2 = list[0].Split(';');

                    res = "";
                    bool skipFirst = true;
                    foreach (string s in arr2)
                    {
                        if (skipFirst)
                        {
                            res += s;

                            skipFirst = false;
                        }
                        else
                        {
                            res += " " + s;
                        }
                        excluded.Add(s);
                    }
                }

                Console.WriteLine(res);
            }
        }
        /*
         * source code reference:
         * https://gist.github.com/jianminchen/872bf70039fa8c61ff208b34a591c8ec
         * 
         */
        public static void buildResult_usingDFS_stack(long total, long storeKBox, int purchaseBoxCount, IList<string> list,
            HashSet<string> excluded)
        {
            // need to complete the task using Queue
            if (purchaseBoxCount == 0 || storeKBox == 0)
                return;

            Stack<Node> stack = new Stack<Node>();

            long start = 1;
            while (start <= storeKBox)
            {
                if (excluded.Contains(start.ToString()))
                {
                    start++;
                    continue;
                }

                long[] arr = new long[2] { 0, start };
                foreach (int val in arr)
                {
                    Node node = new Node();

                    node.length = start;
                    if (val != 0)
                    {
                        node.purchaseBoxes = val.ToString();
                        node.boxCount = 1;
                        node.sum = start;
                    }
                    else
                    {
                        node.purchaseBoxes = "";
                        node.boxCount = 0;
                        node.sum = 0;
                    }

                    stack.Push(node);
                }
                break;
            }

            while (stack.Count > 0)
            {
                Node node = (Node)stack.Pop();

                int boxCount = node.boxCount;

                long len = node.length;
                long sum = node.sum;

                string s1 = node.purchaseBoxes;

                if (boxCount == purchaseBoxCount)
                {
                    if (sum == total)
                    {
                        list.Add(s1);
                        return;
                    }
                }
                else if ((len + 1) <= storeKBox && sum < total)  // more pruning - sum < total - time out issues 8/28/2016 
                {
                    long index = len + 1;
                    while (index <= storeKBox)
                    {
                        if (excluded.Contains(index.ToString()))
                        {
                            index++;
                            continue;
                        }

                        long[] tmpArr = new long[2] { 0, index };
                        foreach (long val in tmpArr)
                        {
                            if (sum + val > long.MaxValue)   // in case the value is too big 
                                continue;

                            Node newNode = new Node();

                            newNode.length = index;  //always increment one

                            newNode.boxCount = val == 0 ? boxCount : (boxCount + 1);
                            newNode.sum = (val == 0) ? sum : (sum + val);

                            newNode.purchaseBoxes = s1 + (val == 0 ? "" : (";" + val.ToString()));

                            stack.Push(newNode);
                        }
                        break;
                    }
                }
            }

            return;
        }

        public static long sum(long n)
        {
            return (long)(n * (n + 1) / 2);
        }
    }
}
