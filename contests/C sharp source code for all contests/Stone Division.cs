using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stoneDivision
{
    class Program
    {
        /*
         * stone division 
         * 
         * 1
         * 12 3
         * 2 3 4
         * 
         * answer: 4 
         * 
         * start coding at 7:18pm
         * 1<=m <= 10 30% of maximum score 
         */
        static void Main(string[] args)
        {
            process();
            //testcase1(); 
        }

        private static void testcase1()
        {
            int n = 12;
            int m = 3;

            int[] setS = new int[3] { 2, 3, 4 };

            Console.WriteLine(maximumMove(n, m, setS));
        }

        private static void process()
        {
            int q = int.Parse(Console.ReadLine());

            for (int i = 0; i < q; i++)
            {
                string[] arr = Console.ReadLine().Split(' ');
                int n = int.Parse(arr[0]);
                int m = int.Parse(arr[1]);

                string[] arr2 = Console.ReadLine().Split(' ');
                int[] setS = Array.ConvertAll(arr2, int.Parse);

                Console.WriteLine(maximumMove(n, m, setS));
            }
        }
        /*
         * review at 8:22pm
         * 
         */
        private static int maximumMove(int n, int m, int[] setS)
        {
            bool[,] isDivisable = new bool[m + 1, m + 1];

            IList<int> list = new List<int>(setS);
            list.Add(n);

            int[] newArr = list.ToArray();

            Array.Sort(newArr);

            for (int i = 0; i < m + 1; i++)
                for (int j = i; j < m + 1; j++)
                {
                    int divisor = newArr[i];
                    int runner = newArr[j];
                    isDivisable[i, j] = (runner % divisor == 0) ? true : false;
                }

            IList<string> chains = new List<string>();
            StringBuilder sb = new StringBuilder();

            int index = m;
            string val = m.ToString();
            getAllChains(chains, sb, isDivisable, newArr, n, m, val + "=" + val);

            // get maximum one here ...
            int maximumMov = int.MinValue;
            foreach (string s in chains)
            {
                string[] steps = s.Split(' ');
                int[] sequence = Array.ConvertAll(steps, int.Parse);
                int count = 0;
                int len = sequence.Length;
                if (len == 1)
                    count = 0;
                int prev = sequence[0];
                int prevCount = 0;
                int groupCount = 1;
                for (int i = 1; i < len; i++)
                {
                    int cur = sequence[i];
                    int setCount = prev / cur;
                    if (i == 1)
                        count++;
                    else
                    {
                        groupCount *= prevCount;
                        count += groupCount;
                    }

                    prev = cur;
                    prevCount = setCount;
                }

                maximumMov = (count > maximumMov) ? count : maximumMov;
            }

            return maximumMov;
        }

        /*
         * 7:37pm
         */
        private static void getAllChains(
            IList<string> chains,
            StringBuilder sb,
            bool[,] isDivisable,
            int[] newArr,
            int n,
            int m,
            string keyword
            )
        {
            string[] rowCol = keyword.Split('=');
            int lastRow = Convert.ToInt32(rowCol[0]);
            int lastCol = Convert.ToInt32(rowCol[1]);

            if (isTheEnd(isDivisable, m, lastRow, lastCol))
            {
                chains.Add(sb.ToString());
                //sb.Clear(); 
                return;
            }

            int start = (lastCol == m) ? 0 : lastRow;

            for (int i = start; i <= lastRow; i++)
            {
                if (isDivisable[i, lastCol] == true)
                {
                    int len = sb.ToString().Length;
                    string next = sb.ToString().Length > 0 ? (" " + newArr[lastCol]) : newArr[lastCol].ToString();
                    sb.Append(next);

                    bool nextOneFound = false;
                    string newKey = "";
                    for (int col = lastCol - 1; col >= 0; col--)
                    {
                        if (isDivisable[i, col])
                        {
                            newKey = i.ToString() + "=" + col.ToString();
                            nextOneFound = true;
                            break;
                        }
                    }

                    if (!nextOneFound)
                    {
                        chains.Add(sb.ToString());
                        //sb.Clear(); 
                        return;
                    }

                    getAllChains(chains, sb, isDivisable, newArr, n, m, newKey);

                    int newLen = sb.ToString().Length;
                    sb.Remove(len, newLen - len);
                }
            }
        }

        private static bool isTheEnd(bool[,] array, int m, int lastRow, int lastCol)
        {
            if (lastRow < 0 || lastCol < 0)
                return true;

            for (int i = 0; i <= lastRow; i++)
            {
                if (array[i, lastCol] == true)
                    return false;
            }

            return true;
        }
    }
}
