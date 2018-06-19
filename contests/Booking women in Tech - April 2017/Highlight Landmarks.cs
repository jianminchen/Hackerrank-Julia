using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlightLandmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var description = Console.ReadLine().Split(' ');
            var number = Convert.ToInt16(Console.ReadLine());
            var destination = Console.ReadLine().Split(' ');

            var highlighted = HighlightDestinations(description, destination);
            Console.WriteLine(highlighted);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static string HighlightDestinations(string[] description, string[] destination)
        {
            var lookup = new HashSet<string>(destination);

            var highlighted = new StringBuilder();

            bool isFirst = true;
            foreach (var word in description)
            {
                if (!isFirst)
                {
                    highlighted.Append(" ");
                }

                var newword = word;
                if (lookup.Contains(word))
                {
                    newword = "<b>" + word + "</b>";
                }

                highlighted.Append(newword);
                isFirst = false;
            }

            return highlighted.ToString();
        }
    }
}
