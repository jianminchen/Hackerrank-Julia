using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{
    internal class CandiesPerColor : IComparable
    {
        public int Candies { get; set; }
        public int ColorId { get; set; }

        /// <summary>
        /// try to make the candies comparison with MinHeap 
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public int CompareTo(object that)
        {
            CandiesPerColor input = (CandiesPerColor)that;
            if (this.Candies > input.Candies) return -1;
            if (this.Candies == input.Candies) return 0;
            return 1;
        }

        public CandiesPerColor(int candies, int colorId)
        {
            ColorId = colorId;
            Candies = candies;
        }
    }

    internal class Box
    {
        public int Id { get; set; }
        public HashSet<int> balls { get; set; } // cannot have same color more than 1              
        public Dictionary<int, int> candiesPerColor { get; set; }
        public MinHeap<CandiesPerColor> candiesPerColor_MaxHeap;

        public int MaximumBalls { get; set; } // but one ball for each color        
        public HashSet<int> ColorIds { get; set; }

        public int CandiesMake { get; set; }

        /// <summary>
        /// initilization of Box object
        /// initilize all objects 
        /// </summary>
        /// <param name="id"></param>
        public Box(int id)
        {
            Id = id;

            balls = new HashSet<int>();
            ColorIds = new HashSet<int>();
            candiesPerColor = new Dictionary<int, int>();
            candiesPerColor_MaxHeap = new MinHeap<CandiesPerColor>();

            CandiesMake = 0;
        }

        /// <summary>
        ///  one copy for all boxes 
        /// </summary>
        /// <param name="colorId"></param>
        /// <param name="candies"></param>
        public void AddCandiesPerColor(int colorId, int candies)
        {
            if (!candiesPerColor.ContainsKey(colorId))
            {
                candiesPerColor_MaxHeap.Insert(new CandiesPerColor(candies, colorId));
                candiesPerColor.Add(colorId, candies);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ball"></param>
        /// <returns></returns>
        public bool AddBall(int color)
        {
            if (ColorIds.Contains(color))
            {
                return false;
            }

            ColorIds.Add(color);

            CandiesMake += candiesPerColor[color];

            return true;
        }

        /// <summary>
        /// add one more ball - what if the balls's total exceeds maximum 
        /// pay using candies 
        /// </summary>
        /// <param name="debit"></param>
        /// <returns></returns>
        public bool CalculateDebit(ref int debit)
        {
            var maximumBalls = this.MaximumBalls;
            var ballsInBox = this.ColorIds.Count();

            // let us take something off
            int diff = ballsInBox + 1 - maximumBalls;
            if (diff > 0)
            {
                debit = diff * diff - (diff - 1) * (diff - 1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    internal class Ball
    {
        public int Id { get; set; }
        public int ColorId { get; set; }

        public Ball(int id)
        {
            Id = id;
        }
    }

    internal class MinHeap<T> where T : IComparable
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

        public T Peek()
        {
            return data[0];
        }

        public int Count
        {
            get { return data.Count; }
        }

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

    static void Main(String[] args)
    {
        ProcessInput();
        //TestMinHeap(); 
    }

    public static void TestMinHeap()
    {
        var maxHeap = new MinHeap<CandiesPerColor>();

        maxHeap.Insert(new CandiesPerColor(1, 0));
        maxHeap.Insert(new CandiesPerColor(3, 0));
        maxHeap.Insert(new CandiesPerColor(7, 0));

        var result = maxHeap.Peek();
    }

    /// <summary>
    /// handle use input
    /// </summary>
    public static void ProcessInput()
    {
        string[] tokens_n = Console.ReadLine().Split(' ');

        int colors = Convert.ToInt32(tokens_n[0]);  // different colors - n 
        int numberOfBoxes = Convert.ToInt32(tokens_n[1]);  // number of boxes 

        var rowNumbers = Console.ReadLine().Split(' ');
        int[] ballsPerColor = Array.ConvertAll(rowNumbers, Int32.Parse);  // balls for each color 

        var rowNumbers_2 = Console.ReadLine().Split(' ');
        int[] maximumBalls = Array.ConvertAll(rowNumbers_2, Int32.Parse); // maximumBalls for each box 

        var candiesCanEarn = new int[colors][]; // candies earned for each box 

        for (int i = 0; i < colors; i++)
        {
            var rowNumbers3 = Console.ReadLine().Split(' ');
            candiesCanEarn[i] = Array.ConvertAll(rowNumbers3, Int32.Parse);
        }

        /*
        int numberOfBoxes = 2;
        int colors = 2;

        int[] ballsPerColor = new int[] {1, 1};
        int[] maximumBalls = new int[]  {0, 2}; 

        var candiesCanEarn = new int[colors][];

        candiesCanEarn[0] = new int[] {1, 7 };
        candiesCanEarn[1] = new int[] {3, 1 };              
        */
        // 
        // Write Your Code Here
        IList<Box> boxes = CreateBoxes(numberOfBoxes, colors, candiesCanEarn, maximumBalls);

        // Add balls to a data structure for easy to look up, consume and track
        var ballsLookup = new Dictionary<int, int>();

        for (int color = 0; color < colors; color++)
        {
            var ballsOneColor = ballsPerColor[color];
            ballsLookup.Add(color, ballsOneColor);
        }

        //         
        long max_candies = long.MinValue;
        addBallToBoxes(ref boxes, ref ballsLookup, ref max_candies);

        Console.WriteLine(max_candies);
    }

    /// <summary>
    /// try to put balls to box and make some candies
    /// </summary>
    /// <param name="boxes"></param>
    /// <param name="balls"></param>
    /// <returns></returns>
    private static void addBallToBoxes(
        ref IList<Box> boxes,
        ref Dictionary<int, int> ballsLookup,
        ref long max_candies)
    {
        long candies = 0;

        while (hasBalls(ballsLookup))
        {
            // need to add       
            int boxId = -1;
            var found = getMaxCandiesPerColorForBox(boxes, ref boxId);

            if (!found)
            {
                break;
            }

            var selected = boxes[boxId];
            var candiesByColor = selected.candiesPerColor_MaxHeap.Peek();
            var chosenColor = candiesByColor.ColorId;
            var candiesAcquired = candiesByColor.Candies;

            if (ballsLookup.ContainsKey(chosenColor) &&
                ballsLookup[chosenColor] > 0
                )
            {
                int number = ballsLookup[chosenColor];

                int debit = 0;
                if (selected.CalculateDebit(ref debit))
                {
                    candies -= debit;
                }

                ballsLookup[chosenColor] = number - 1;
                candies += candiesAcquired;
                boxes[boxId].AddBall(chosenColor);
            }

            // remove the color setting since one box only can have one ball for one color at most
            boxes[boxId].candiesPerColor_MaxHeap.ExtractMin();

            max_candies = Math.Max(candies, max_candies);
        }
    }

    private static bool hasBalls(Dictionary<int, int> ballsLookup)
    {
        foreach (var key in ballsLookup.Keys)
        {
            if (ballsLookup[key] > 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// need to consider maximum candies per color, and also count debit
    /// if the balls in the box exceeds the maximum count 
    /// </summary>
    /// <param name="boxes"></param>
    /// <param name="selectedId"></param>
    /// <returns></returns>
    private static bool getMaxCandiesPerColorForBox(IList<Box> boxes, ref int selectedId)
    {
        int max = int.MinValue;
        selectedId = 0;
        bool foundOne = false;
        for (int i = 0; i < boxes.Count(); i++)
        {
            var box = boxes[i];
            var heap = boxes[i].candiesPerColor_MaxHeap;

            if (heap.Count == 0)
            {
                continue;
            }

            var current = heap.Peek();
            var candies = current.Candies;

            int debit = 0;
            if (box.CalculateDebit(ref debit))
            {
                candies -= debit;
            }

            // skip those negative ones 
            if (candies <= 0)
            {
                continue;
            }

            if (candies > max)
            {
                selectedId = i;
                max = candies;
            }

            foundOne = true;
        }

        return foundOne;
    }

    private static IList<Box> CreateBoxes(int numberOfBoxes, int colors, int[][] candiesCanEarn, int[] maximumBalls)
    {
        IList<Box> boxes = new List<Box>();

        for (int i = 0; i < numberOfBoxes; i++)
        {
            var current = new Box(i);
            for (int color = 0; color < colors; color++)
            {
                current.AddCandiesPerColor(color, candiesCanEarn[color][i]);
            }

            current.MaximumBalls = maximumBalls[i];

            boxes.Add(current);
        }

        return boxes;
    }
}