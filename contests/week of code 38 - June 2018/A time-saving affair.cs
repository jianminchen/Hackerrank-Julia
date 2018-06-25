using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

class Solution
{
    /// <summary>
    /// code review June 24, 2018
    /// I figured out the user case 
    /// - last step - no need to wait for greenlight - by looking up discussion
    /// fix logic on line 55
    /// add constraint: end != numberOfJunctions
    /// </summary>
    /// <param name="numberOfJunctions"></param>
    /// <param name="kSecondSignal"></param>
    /// <param name="numberOfRoads"></param>
    /// <param name="roads"></param>
    /// <returns></returns>
    static long leastTimeToInterview(int numberOfJunctions, int kSecondSignal, int numberOfRoads, IList<string[]> roads)
    {
        // Return the least amount of time needed to reach the interview location in seconds.
        var roadsDict = getRoadsDict(roads);

        var distanceMap = new Dictionary<int, int>();
        var queue = new Queue<int[]>();

        // add all those numbers to the queue
        var firstOutward = roadsDict[1];
        distanceMap.Add(1, 0);

        foreach (var item in firstOutward)
        {
            queue.Enqueue(new int[] { 1, item.Key });
        }

        while (queue.Count > 0)
        {
            var item = queue.Dequeue();
            var start = item[0];
            var end = item[1];

            var timeTravelled = roadsDict[start][end];
            var startTime = distanceMap[start];
            var actualTime = startTime + timeTravelled;
            var waitForRedLight = end != numberOfJunctions && actualTime / kSecondSignal % 2 == 1; // exclude last one
            var delayedToGreen = actualTime;

            if (waitForRedLight)
                delayedToGreen = (actualTime + kSecondSignal) / kSecondSignal * kSecondSignal; // (5 + 4)/ 4 * 2 = 8 => 5 -> 8

            if (!distanceMap.ContainsKey(end) || distanceMap[end] > delayedToGreen)
            {
                if (!distanceMap.ContainsKey(end))
                    distanceMap.Add(end, delayedToGreen);

                distanceMap[end] = delayedToGreen;  // fix the bug 

                //Console.WriteLine("distanceMap" + end + " = "+ delayedToGreen);

                var routes = roadsDict[end];

                foreach (var route in routes)
                {
                    var nextKey = route.Key;
                    if (nextKey != end && // exclude self-loop
                        (!distanceMap.ContainsKey(nextKey) || distanceMap[nextKey] > delayedToGreen))
                    {
                        queue.Enqueue(new int[] { end, route.Key });
                    }
                }
            }
        }

        return distanceMap[numberOfJunctions];
    }

    /// <summary>
    /// I figured out the user case on self-loop by reading the comment - 6/24/2018
    /// since I have no clue that I only pass test case 1, run time error on first 2 and 4, 
    /// and then wrong answer for 3 and 5.
    /// I checked the discussion and then understand that self-loop means that there are maybe 
    /// more than two routes from A to B.
    /// </summary>
    /// <param name="roads"></param>
    /// <returns></returns>
    private static Dictionary<int, Dictionary<int, int>> getRoadsDict(IList<string[]> roads)
    {
        var roadsDict = new Dictionary<int, Dictionary<int, int>>();

        var length = roads.Count;

        foreach (var item in roads)
        {
            var endA = Convert.ToInt32(item[0]);
            var endB = Convert.ToInt32(item[1]);
            var dist = Convert.ToInt32(item[2]);

            if (!roadsDict.ContainsKey(endA))
            {
                roadsDict.Add(endA, new Dictionary<int, int>());
            }

            if (!roadsDict.ContainsKey(endB))
            {
                roadsDict.Add(endB, new Dictionary<int, int>());
            }

            if (!roadsDict[endA].ContainsKey(endB))
                roadsDict[endA].Add(endB, dist);
            else
            {
                var current = roadsDict[endA][endB];
                if (current > dist)
                    roadsDict[endA][endB] = dist;
            }

            if (!roadsDict[endB].ContainsKey(endA))
                roadsDict[endB].Add(endA, dist);
            else
            {
                var current = roadsDict[endB][endA];
                if (current > dist)
                    roadsDict[endB][endA] = dist;
            }
        }

        return roadsDict;
    }

    static void Main(string[] args)
    {
        //runTestcase();

        process();
    }

    private static void runTestcase()
    {
        var numberOfJunctions = 7;
        var kSecondsSignal = 4;
        var numberOfRoads = 7;
        var roads = new List<string[]>();

        roads.Add(new string[] { "1", "2", "3" });
        roads.Add(new string[] { "2", "3", "1" });
        roads.Add(new string[] { "1", "4", "4" });
        roads.Add(new string[] { "4", "6", "7" });
        roads.Add(new string[] { "7", "5", "2" });
        roads.Add(new string[] { "3", "5", "1" });
        roads.Add(new string[] { "4", "5", "5" });

        long result = leastTimeToInterview(numberOfJunctions, kSecondsSignal, numberOfRoads, roads);
    }

    private static void process()
    {
        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

        int numberOfJunctions = Convert.ToInt32(Console.ReadLine());

        int kSecondsSignal = Convert.ToInt32(Console.ReadLine());

        int numberOfRoads = Convert.ToInt32(Console.ReadLine());

        var roads = new List<string[]>();

        for (int index = 0; index < numberOfRoads; index++)
        {
            var input = Console.ReadLine();
            var split = input.Split(' ');

            roads.Add(split);
        }

        long result = leastTimeToInterview(numberOfJunctions, kSecondsSignal, numberOfRoads, roads);

        textWriter.WriteLine(result);

        textWriter.Flush();
        textWriter.Close();
    }
}
