using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingAvailable
{
    /// <summary>
    /// https://www.hackerrank.com/contests/booking-womenintech/challenges/b-available
    /// 
    /// </summary>
    class Program
    {
        internal class PriceOptions
        {
            public IList<Price> SpecialPrices { get; set; }
        }

        internal class Price
        {
            public int PriceValue { get; set; }
            public int MinimumDays { get; set; }
            public int MaximumDays { get; set; }

            public Price(int value)
            {
                PriceValue = value;
            }
        }

        internal class MinimumDays
        {
            public IList<int> days { get; set; }
        }

        internal class MaximumDays
        {
            public IList<int> days { get; set; }
        }

        internal class Query
        {
            public int CheckinDate { get; set; }
            public int NumberOfNights { get; set; }
        }

        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase(); 
        }

        public static void RunTestcase()
        {
            var totalNumberNights = 8;     // N
            var numberOfPrices = 4;     // M      
            var numberOfQueries = 1;    // Q

            var priceOptions = new PriceOptions[numberOfPrices];

            int[][] values = new int[4][];
            values[0] = Array.ConvertAll("100 120 90 110 100 90 130 0".Split(' '), int.Parse);
            values[1] = Array.ConvertAll("0 0 0 0 0 60 60 0".Split(' '), int.Parse);
            values[2] = Array.ConvertAll("90 100 80 90 70 80 110 110".Split(' '), int.Parse);
            values[3] = Array.ConvertAll("88 108 78 98 88 78 118 0".Split(' '), int.Parse);

            // first M lines 
            for (int i = 0; i < numberOfPrices; i++)
            {
                priceOptions[i] = new PriceOptions();  // added after Hackerrank complained runtime error
                priceOptions[i].SpecialPrices = new List<Price>(); // added after Hackerrank complained runtime error               

                foreach (var item in values[i])
                {
                    priceOptions[i].SpecialPrices.Add(new Price(item));
                }
            }

            int[][] minimumDays = new int[4][];

            minimumDays[0] = Array.ConvertAll("0 0 0 0 0 0 0 0".Split(' '), int.Parse);
            minimumDays[1] = Array.ConvertAll("0 0 0 0 0 0 0 0".Split(' '), int.Parse);
            minimumDays[2] = Array.ConvertAll("5 5 5 5 5 5 5 5".Split(' '), int.Parse);
            minimumDays[3] = Array.ConvertAll("0 0 0 0 0 0 0 0".Split(' '), int.Parse);

            // first M lines 
            for (int i = 0; i < numberOfPrices; i++)
            {
                int index = 0;
                foreach (var item in minimumDays[i])
                {
                    priceOptions[i].SpecialPrices[index].MinimumDays = item;

                    index++;
                }
            }

            int[][] maximumDays = new int[4][];

            maximumDays[0] = Array.ConvertAll("8 8 8 8 8 8 8 8".Split(' '), int.Parse);
            maximumDays[1] = Array.ConvertAll("8 8 8 8 8 2 2 8".Split(' '), int.Parse);
            maximumDays[2] = Array.ConvertAll("8 8 8 8 8 8 8 8".Split(' '), int.Parse);
            maximumDays[3] = Array.ConvertAll("8 8 8 8 8 8 8 8".Split(' '), int.Parse);

            // first M lines 
            for (int i = 0; i < numberOfPrices; i++)
            {
                int index = 0;
                foreach (var item in maximumDays[i])
                {
                    priceOptions[i].SpecialPrices[index].MaximumDays = item;

                    index++;
                }
            }


            var queries = new Query[numberOfQueries];

            // sort priceOptions by priceValue ascending order           

            for (int i = 0; i < numberOfQueries; i++)
            {
                queries[i] = new Query(); // added after runtime error

                var days = new int[] { 1, 7 };

                queries[i].CheckinDate = days[0];
                queries[i].NumberOfNights = days[1];

                var minimumPrices = CalculateMinimumPrices(priceOptions, queries[i]);
                Console.WriteLine(minimumPrices);
            }
        }

        public static void ProcessInput()
        {
            var numbers = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

            var totalNumberNights = numbers[0];     // N
            var numberOfPrices = numbers[1];     // M      
            var numberOfQueries = numbers[2];    // Q

            var priceOptions = new PriceOptions[numberOfPrices];

            // first M lines 
            for (int i = 0; i < numberOfPrices; i++)
            {
                priceOptions[i] = new PriceOptions();  // added after Hackerrank complained runtime error
                priceOptions[i].SpecialPrices = new List<Price>(); // added after Hackerrank complained runtime error

                var values = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                foreach (var item in values)
                {
                    priceOptions[i].SpecialPrices.Add(new Price(item));
                }
            }

            // first M lines 
            for (int i = 0; i < numberOfPrices; i++)
            {
                var values = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                int index = 0;
                foreach (var item in values)
                {
                    priceOptions[i].SpecialPrices[index].MinimumDays = item;

                    index++;
                }
            }

            var maximumDays = new MaximumDays[numberOfPrices];
            // first M lines 
            for (int i = 0; i < numberOfPrices; i++)
            {
                var values = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                int index = 0;
                foreach (var item in values)
                {
                    priceOptions[i].SpecialPrices[index].MaximumDays = item;

                    index++;
                }
            }

            var queries = new Query[numberOfQueries];

            // number of queries - 1000000 
            /*
            var memo = new Dictionary<int, IList<PriceOptions>>(); 

            for(int i = 0; i < totalNumberNights; i++)
            {
                foreach(var item in priceOptions)
                {
                    var priceOption = item.SpecialPrices[i];  // ? look into later
                }
            }
             * */
            // too late for the hacking 

            for (int i = 0; i < numberOfQueries; i++)
            {
                queries[i] = new Query(); // added after runtime error

                var values = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);

                queries[i].CheckinDate = values[0];
                queries[i].NumberOfNights = values[1];

                // ? 
                /*
                var key = values[0] + "," + values[1]; 
                if(memo.ContainsKey(key))
                {
                    Console.WriteLine(memo[key]);
                    return; 
                }
                */

                var minimumPrices = CalculateMinimumPrices(priceOptions, queries[i]);

                //memo.Add(key, minimumPrices); 

                Console.WriteLine(minimumPrices);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priceOptions"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static int CalculateMinimumPrices(PriceOptions[] priceOptions, Query query)
        {
            const int CouldNotFound = -1;

            int checkinDate = query.CheckinDate;
            int numberOfNights = query.NumberOfNights;

            // need to work on queries - maximum number - 1000000 upper bound
            // need to work on 300 nights upperbound
            // need to work on 400 prices upperbound
            // total will be 1000000 * 300 * 400 = 1.2 * 10,000,000,000 times 
            // how to reduce duplicated work here...

            // sorted by priceOptions by PriceValue

            // go over each day, maximum 300 nights <- think about what data structure here!          
            int minimumPrices = 0;
            for (int i = 0; i < numberOfNights; i++)
            {
                var date = checkinDate + i - 1;  // run time error - add "-1"
                int minimumValue = int.MaxValue;

                // maximum price option - 400 prices here -> what data structure 
                foreach (var priceOption in priceOptions)
                {
                    var dayPrice = priceOption.SpecialPrices[date];

                    int minStay = dayPrice.MinimumDays;
                    int maxStay = dayPrice.MaximumDays;
                    int value = dayPrice.PriceValue;

                    if (value > 0 && numberOfNights >= minStay && numberOfNights <= maxStay)
                    {
                        minimumValue = (value < minimumValue) ? value : minimumValue;
                    }
                }

                // could not find the booking, like test case the third day 2017-05-08
                if (minimumValue == int.MaxValue)
                {
                    return CouldNotFound;
                }

                minimumPrices += minimumValue;
            }

            return minimumPrices;
        }
    }
}
