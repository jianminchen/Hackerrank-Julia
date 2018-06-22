using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cityConstruction
{
    class cityConstruction
    {
        internal class City
        {
            public int Id { get; set; }
            //public HashSet<Road> roadFrom { get; set; }
            public HashSet<int> roadToCityId { get; set; }

            public City(int id)
            {
                this.Id = id;

                //roadFrom = new HashSet<Road>();
                roadToCityId = new HashSet<int>();
            }

            public void AddRoad(Road road)
            {
                //roadFrom.Add(road);
                roadToCityId.Add(road.To.Id);
            }
        }

        internal class Road
        {
            public City From { get; set; }
            public City To { get; set; }

            public int Id { get; set; }

            public Road(int id)
            {
                Id = id;
            }
        }

        static void Main(String[] args)
        {
            ProcesInput();
            //RunTestcase(); 
            //RunTestcase2(); 
        }

        public static void RunTestcase()
        {
            int numberOfCities = 4;
            int numberOfUniqueDirectionRoads = 4;

            IList<int[]> roads = new List<int[]>();
            roads.Add(new int[] { 1, 2 });
            roads.Add(new int[] { 1, 3 });
            roads.Add(new int[] { 2, 4 });
            roads.Add(new int[] { 3, 4 });

            int queries = 5;

            IList<int[]> queriesDetail = new List<int[]>();
            queriesDetail.Add(new int[] { 1, 2, 0 });
            queriesDetail.Add(new int[] { 2, 3, 5 });
            queriesDetail.Add(new int[] { 2, 1, 5 });
            queriesDetail.Add(new int[] { 1, 1, 1 });
            queriesDetail.Add(new int[] { 2, 6, 4 });

            var result = ProcessQueries(roads, queriesDetail, numberOfCities, numberOfUniqueDirectionRoads);

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void RunTestcase2()
        {
            int numberOfCities = 5;
            int numberOfUniqueDirectionRoads = 4;

            IList<int[]> roads = new List<int[]>();
            roads.Add(new int[] { 1, 2 });
            roads.Add(new int[] { 2, 3 });
            roads.Add(new int[] { 3, 1 });
            roads.Add(new int[] { 4, 5 });

            int queries = 1;

            IList<int[]> queriesDetail = new List<int[]>();
            queriesDetail.Add(new int[] { 2, 1, 5 });


            var result = ProcessQueries(roads, queriesDetail, numberOfCities, numberOfUniqueDirectionRoads);

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public static void ProcesInput()
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            int numberOfCities = Convert.ToInt32(tokens_n[0]);
            int numberOfUniqueDirectionRoads = Convert.ToInt32(tokens_n[1]);

            IList<int[]> roads = new List<int[]>();
            for (int i = 0; i < numberOfUniqueDirectionRoads; i++)
            {
                var row = Console.ReadLine().Split(' ');
                int u = Convert.ToInt32(row[0]);
                int v = Convert.ToInt32(row[1]);

                roads.Add(new int[] { u, v });
            }

            int queries = Convert.ToInt32(Console.ReadLine());

            IList<int[]> queriesDetail = new List<int[]>();
            for (int i = 0; i < queries; i++)
            {
                var row = Array.ConvertAll(Console.ReadLine().Split(' '), int.Parse);
                queriesDetail.Add(row);
            }

            var result = ProcessQueries(roads, queriesDetail, numberOfCities, numberOfUniqueDirectionRoads);

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roads"></param>
        /// <param name="queriesDetail"></param>
        /// <returns></returns>
        public static IList<string> ProcessQueries(
            IList<int[]> roads,
            IList<int[]> queriesDetail,
            int numberOfCities,
            int numberOfUniqueDirectionRoads)
        {
            IList<string> result = new List<string>();

            // add cities 
            IList<City> cities = addCities(numberOfCities);

            // add roads
            IList<Road> roadsLookup = addRoads(roads, cities);

            int newCitieId = numberOfCities + 1;
            int newRoadId = numberOfUniqueDirectionRoads + 1;

            int possibleCities = 50000 + 1; //  cities <= 50000 original                              


            // Let us work on the queries
            foreach (var query in queriesDetail)
            {
                int id1 = query[0];
                int id2 = query[1];
                int id3 = query[2];

                // query type id1 == 1 - construct a road
                // query type id1 == 2 - query the road
                // construct a road
                //    id3 = 0; from city id2 to a new city, for example, 1 2 0  
                //    id3 = 1; construct a road from new city to the city id2, for example, 1 1 1


                // query type id1 == 1 - construct a road
                // 1 2 0 - construct a road from city 2 to a new city, id = 5
                bool constructANewRoad = id1 == 1;
                if (constructANewRoad)
                {
                    // create a new city first
                    var newCity = new City(newCitieId++);
                    cities.Add(newCity);

                    // set up a road
                    bool toNewCity = id3 == 0;
                    bool fromNewCity = id3 == 1;
                    var newRoad = new Road(newRoadId++);
                    int cityIdIndex = id2 - 1; // be careful 

                    newRoad.From = fromNewCity ? newCity : cities[cityIdIndex];
                    newRoad.To = toNewCity ? newCity : cities[cityIdIndex];

                    if (toNewCity)
                    {
                        cities[cityIdIndex].AddRoad(newRoad);
                    }
                    else
                    {
                        newCity.AddRoad(newRoad);
                    }

                    //resetMemoForUnfound(hasMemo); // reset unfound ones to zero 
                }

                // query type id1 == 2 - if there is a road from city id2 to id3 // 2 3 5
                bool queryARoad = id1 == 2;
                if (queryARoad)
                {
                    int fromCityId = id2;
                    int toCityId = id3;

                    var visitedNode = new HashSet<int>();
                    visitedNode.Add(fromCityId);

                    BitArray memoHaveRoadFromTo = new BitArray(possibleCities, false);
                    BitArray hasMemo = new BitArray(possibleCities, false);

                    // query 
                    bool roadFind = findUniDirectionRoad(cities, fromCityId, toCityId, hasMemo, memoHaveRoadFromTo, visitedNode);
                    result.Add(roadFind ? "Yes" : "No");
                }
            }

            return result;
        }



        /// <summary>
        /// be careful deadloop - need to check recursive depths 
        /// </summary>
        /// <param name="cities"></param>
        /// <param name="fromCity"></param>
        /// <param name="toCity"></param>
        /// <param name="memoFromTo"></param>
        /// <param name="recursiveDepth"></param>
        /// <returns></returns>
        private static bool findUniDirectionRoad(
            IList<City> cities,
            int fromCity,
            int toCity,
            BitArray hasMemo,
            BitArray memoFromTo,
            HashSet<int> visitedNode)
        {
            // base case - terminate the loop
            if (fromCity == toCity)
            {
                hasMemo.Set(fromCity, true);
                memoFromTo.Set(fromCity, true);
                return true;
            }

            if (hasMemo.Get(fromCity) == true)
            {  // 1 - true  -1 false
                var result = memoFromTo[fromCity];
                return result;
            }

            var city = cities[fromCity - 1];  // look up from cities 

            int count = city.roadToCityId.Count;

            // no exit 
            if (count == 0)
            {
                hasMemo.Set(fromCity, true);
                memoFromTo.Set(fromCity, false);
                return false;
            }

            bool foundOne = false;
            bool allVisited = true;

            if (city.roadToCityId.Contains(toCity))
            {
                hasMemo.Set(fromCity, true);
                memoFromTo.Set(fromCity, true);
                return true;
            }

            foreach (var visitId in city.roadToCityId)
            {
                if (visitedNode.Contains(visitId))
                {
                    continue;
                }

                allVisited = false;
                var newSet = new HashSet<int>(visitedNode);

                newSet.Add(visitId);
                var found = findUniDirectionRoad(cities, visitId, toCity, hasMemo, memoFromTo, newSet);
                if (found)
                {
                    foundOne = true;
                    break;
                }
            }

            if (allVisited)
            {
                hasMemo.Set(fromCity, true);
                memoFromTo.Set(fromCity, false);
                return false;
            }

            hasMemo.Set(fromCity, true);
            memoFromTo.Set(fromCity, foundOne);

            return foundOne;
        }

        private static IList<City> addCities(int numberOfCities)
        {
            IList<City> cities = new List<City>();

            for (int id = 1; id < numberOfCities + 1; id++)  // 1 <= u, v <= n
            {
                var city = new City(id);
                cities.Add(city);
            }

            return cities;
        }

        /// <summary>
        /// be careful about city id - id = 1, store in 0 - index 
        /// </summary>
        /// <param name="roads"></param>
        /// <param name="cities"></param>
        /// <returns></returns>
        private static IList<Road> addRoads(IList<int[]> roads, IList<City> cities)
        {
            IList<Road> roadsLookup = new List<Road>();

            int roadId = 0;
            foreach (var road in roads)
            {
                var from = road[0] - 1;
                var to = road[1] - 1;

                var currentRoad = new Road(roadId);
                currentRoad.From = cities[from];
                currentRoad.To = cities[to];

                cities[from].AddRoad(currentRoad);

                roadsLookup.Add(currentRoad);

                roadId++;
            }

            return roadsLookup;
        }
    }
}
