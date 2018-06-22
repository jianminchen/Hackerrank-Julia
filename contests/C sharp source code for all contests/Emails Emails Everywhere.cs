using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email
{
    class email
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            var queries = 3;

            const int MAX = 100000 + 1;
            var queueByPriority = new Queue<string>[MAX]; // 
            int highestPriority = -1;
            for (int i = 0; i < MAX; i++)
            {
                queueByPriority[i] = new Queue<string>();
            }

            var messages = new string[] { "store email1 1", "store email2 10", "get_next_email" };

            IList<string> result = new List<string>();

            var minHeap = new MinHeap<int>();

            for (int i = 0; i < queries; i++)
            {
                var command = messages[i].Split(' ');

                if (command.Length == 1)
                {
                    // get next email
                    result.Add(RemoveFirstEmail(queueByPriority, MAX, minHeap));
                }
                else
                {
                    var message = command[1];
                    var priority = command[2];

                    SaveMessageToQueue(queueByPriority, message, priority, minHeap);
                }
            }
        }

        public static void ProcessInput()
        {
            var queries = Convert.ToInt32(Console.ReadLine());

            const int MAX = 100001;
            var queueByPriority = new Queue<string>[MAX];

            for (int i = 0; i < MAX; i++)
            {
                queueByPriority[i] = new Queue<string>();
            }

            MinHeap<int> minHeap = new MinHeap<int>();

            for (int i = 0; i < queries; i++)
            {
                var command = Console.ReadLine().Split(' ');
                if (command.Length == 1)
                {
                    // get next email
                    Console.WriteLine(RemoveFirstEmail(queueByPriority, MAX, minHeap));
                }
                else
                {
                    var message = command[1];
                    var priority = command[2];

                    SaveMessageToQueue(queueByPriority, message, priority, minHeap);
                }
            }
        }

        /// <summary>
        /// save - O(1), easy to find the queue, which queue to save - by priority number
        /// </summary>
        /// <param name="queueByPriority"></param>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        public static void SaveMessageToQueue(Queue<string>[] queueByPriority, string message, string priority, MinHeap<int> minHeap)
        {
            int index = Convert.ToInt32(priority);
            queueByPriority[index].Enqueue(message);

            if (minHeap.Count == 0)
            {
                minHeap.Insert(getNegative(index));
            }
            else
            {
                if (queueByPriority[index].Count == 1)
                {
                    minHeap.Insert(getNegative(index));
                }
            }
        }

        private static int getNegative(int priority)
        {
            return -1 * priority;
        }

        /// <summary>
        /// size is 100000 - need to do time complexity analysis
        /// it is O(n) algorithm to remove, not efficient
        /// </summary>
        /// <param name="queueByPriority"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string RemoveFirstEmail(Queue<string>[] queueByPriority, int size, MinHeap<int> minHeap)
        {
            var message = "-1";

            if (minHeap.Count == 0)
            {
                return message;
            }

            var highestPriority = getHighestPriority(minHeap.Peek());
            Queue<string> current = queueByPriority[highestPriority];

            message = current.Dequeue();

            if (current.Count == 0)
            {
                minHeap.ExtractMin();
            }

            return message;
        }

        public static int getHighestPriority(int value)
        {
            return -1 * value;
        }
    }

    /// <summary>
    /// original code from here:
    /// http://allanrbo.blogspot.ca/2011/12/simple-heap-implementation-priority.html
    /// 
    /// April 22, 2017
    /// Julia did code review ont he code 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MinHeap<T> where T : IComparable
    {
        private List<T> data = new List<T>();

        public void Insert(T o)
        {
            data.Add(o);

            int i = data.Count - 1;
            while (i > 0)
            {
                int j = (i + 1) / 2 - 1;

                // Check if the invariant holds for the element in data[i]  
                T v = data[j];
                if (v.CompareTo(data[i]) < 0 || v.CompareTo(data[i]) == 0)
                {
                    break;
                }

                // Swap the elements  
                T tmp = data[i];
                data[i] = data[j];
                data[j] = tmp;

                i = j;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T ExtractMin()
        {
            if (data.Count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            T min = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            this.MinHeapify(0);
            return min;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return data[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return data.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        private void MinHeapify(int i)
        {
            int smallest;
            int l = 2 * (i + 1) - 1;
            int r = 2 * (i + 1) - 1 + 1;

            if (l < data.Count && (data[l].CompareTo(data[i]) < 0))
            {
                smallest = l;
            }
            else
            {
                smallest = i;
            }

            if (r < data.Count && (data[r].CompareTo(data[smallest]) < 0))
            {
                smallest = r;
            }

            if (smallest != i)
            {
                T tmp = data[i];
                data[i] = data[smallest];
                data[smallest] = tmp;
                this.MinHeapify(smallest);
            }
        }
    }
}
