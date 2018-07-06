using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Solution
{



    static void Main(String[] args)
    {
        ProcessInput();
        //RunTestcase(); 
    }

    public static void RunTestcase()
    {
        //var result = timeCompare("12:00AM", "01:01AM"); 
        var result = timeCompare("02:00PM", "11:09PM");
    }

    public static void ProcessInput()
    {
        int q = Convert.ToInt32(Console.ReadLine());
        for (int a0 = 0; a0 < q; a0++)
        {
            string[] tokens_t1 = Console.ReadLine().Split(' ');
            string t1 = tokens_t1[0];
            string t2 = tokens_t1[1];
            string result = timeCompare(t1, t2);
            Console.WriteLine(result);
        }
    }

    static string timeCompare(string t1, string t2)
    {
        // Complete this function
        int time1 = CalculateMinutesPassedAfterMidnight(t1);
        int time2 = CalculateMinutesPassedAfterMidnight(t2);

        if (time1 <= time2)
        {
            return "First";
        }
        else
        {
            return "Second";
        }
    }

    static int CalculateMinutesPassedAfterMidnight(string time)
    {
        bool isAm = time[5] == 'A';
        var hoursString = "00,01,02,03,04,05,06,07,08,09,10,11,12";
        string[] hoursSymbol = hoursString.Split(',');

        string hours = time.Substring(0, 2);
        string minutes = time.Substring(3, 2);

        int inMinutes = 0;

        int hh = Array.IndexOf(hoursSymbol, hours);

        // calculate hours 
        bool is12AmHour = hh == 12 && isAm;
        bool is12PmHour = hh == 12 && !isAm;

        if (is12AmHour || is12PmHour)
        {
            hh = 0;
        }

        if (!isAm)
        {
            hh += 12;
        }

        inMinutes = hh * 60;

        var first10Minutes = "00,01,02,03,04,05,06,07,08,09".Split(',');
        int mm = Array.IndexOf(first10Minutes, minutes);
        if (mm >= 0)
        {
            inMinutes += mm;
        }
        else
        {
            inMinutes += Convert.ToInt16(minutes);
        }

        return inMinutes;
    }

}
