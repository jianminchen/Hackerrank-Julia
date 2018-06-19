using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFileCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInput();
            //RunTestcase();
        }

        public static void RunTestcase()
        {
            var operation = new string[] { "crt", "del", "rnm" };

            var creations = new Dictionary<string, SortedSet<int>>();
            var deletions = new Dictionary<string, SortedSet<int>>();

            var commandList = new string[] { 
                "crt phonebook", 
                "crt phonebook",
                "crt phonebook",               
                "crt todo",
                "crt phonebook", 
                "del phonebook", 
                "del phonebook(2)", 
                "crt phonebook", 
                "crt phonebook",
                "crt phonebook",  
                "rnm phonebook(2) todo" };
            // Process each command
            for (int index = 0; index < commandList.Length; index++)
            {
                string command = commandList[index];

                var commands = command.Split(' ');
                var action = commands[0];
                var fileName = commands[1];
                var targetName = commands.Length >= 3 ? commands[2] : "";

                bool isCrt = action.CompareTo(operation[0]) == 0;
                bool isDel = action.CompareTo(operation[1]) == 0;
                bool isRnm = action.CompareTo(operation[2]) == 0;

                // duplicate here 
                string rename = "r ";

                if (isDel || isRnm)
                {
                    handleRemoveFile(creations, deletions, fileName);
                    if (isDel)
                    {
                        Console.WriteLine("- " + fileName);
                    }
                    if (isRnm)
                    {
                        rename += fileName;
                    }
                }
                // crt command
                if (isCrt || isRnm)
                {
                    // add new file 
                    var newFileName = isCrt ? fileName : targetName;
                    var output = handleAddFile(creations, deletions, newFileName);
                    if (isCrt)
                    {
                        Console.WriteLine("+ " + output);
                    }

                    if (isRnm)
                    {
                        rename += " -> " + output;
                        Console.WriteLine(rename);
                    }
                }
            }
        }

        public static void ProcessInput()
        {
            int queries = Convert.ToInt32(Console.ReadLine());

            var operation = new string[] { "crt", "del", "rnm" };

            var creations = new Dictionary<string, SortedSet<int>>();
            var deletions = new Dictionary<string, SortedSet<int>>();

            // Process each command
            for (int index = 0; index < queries; index++)
            {
                string command = Console.ReadLine();

                var commands = command.Split(' ');
                var action = commands[0];
                var fileName = commands[1];
                var targetName = commands.Length >= 3 ? commands[2] : "";

                bool isCrt = action.CompareTo(operation[0]) == 0;
                bool isDel = action.CompareTo(operation[1]) == 0;
                bool isRnm = action.CompareTo(operation[2]) == 0;

                // duplicate here 
                string rename = "r ";

                if (isDel || isRnm)
                {
                    handleRemoveFile(creations, deletions, fileName);
                    if (isDel)
                    {
                        Console.WriteLine("- " + fileName);
                    }
                    if (isRnm)
                    {
                        rename += fileName;
                    }
                }
                // crt command
                if (isCrt || isRnm)
                {
                    // add new file 
                    var newFileName = isCrt ? fileName : targetName;
                    var output = handleAddFile(creations, deletions, newFileName);
                    if (isCrt)
                    {
                        Console.WriteLine("+ " + output);
                    }

                    if (isRnm)
                    {
                        rename += " -> " + output;
                        Console.WriteLine(rename);
                    }
                }
            }
        }

        private static bool fileNameWithValue(string s)
        {
            return s.IndexOf('(') >= 0;
        }

        private static string[] fileNameParse(string s)
        {
            int value = s.IndexOf('(');

            if (value >= 0)
            {
                int valueClose = s.IndexOf(')');
                var number = s.Substring(value + 1, valueClose - value - 1);
                return new string[] { s.Substring(0, value), number.ToString() };
            }

            return new string[0];
        }

        private static string handleAddFile(Dictionary<string, SortedSet<int>> creations,
           Dictionary<string, SortedSet<int>> deletions, string fileName)
        {
            if (!deletions.ContainsKey(fileName))
            {
                if (!creations.ContainsKey(fileName))
                {
                    var newSet = new SortedSet<int>();
                    newSet.Add(0);
                    creations.Add(fileName, newSet);

                    return constructAdd(fileName, 0);
                }
                else
                {
                    var set = creations[fileName];
                    int value = set.Last();
                    set.Add(value + 1);
                    creations[fileName] = set;

                    return constructAdd(fileName, value + 1);
                }
            }
            else
            {
                var set = deletions[fileName];
                int value = set.First();
                set.Remove(value);

                if (set.Count == 0)
                {
                    deletions.Remove(fileName);
                }
                else
                {
                    deletions[fileName] = set;
                }

                if (!creations.ContainsKey(fileName))
                {
                    var newSet = new SortedSet<int>();
                    newSet.Add(value);
                    creations.Add(fileName, newSet);
                }
                else
                {
                    var creationSet = creations[fileName];
                    creationSet.Add(value);
                    creations[fileName] = creationSet;
                }

                return constructAdd(fileName, value);
            }
        }

        private static string constructAdd(string fileName, int value)
        {
            var message = fileName;
            if (value > 0)
            {
                message += "(" + value.ToString() + ")";
            }

            return message;
        }

        private static void handleRemoveFile(Dictionary<string, SortedSet<int>> creations,
          Dictionary<string, SortedSet<int>> deletions, string fileName)
        {
            bool containValue = fileNameWithValue(fileName);
            var nameValue = fileNameParse(fileName);
            var realName = containValue ? nameValue[0] : fileName;
            // remove the file 
            var set = creations[realName];

            int value = 0;

            if (containValue)
            {
                value = Convert.ToInt32(nameValue[1]);
            }

            set.Remove(value);
            creations[fileName] = set;

            if (!deletions.ContainsKey(realName))
            {
                var newSet = new SortedSet<int>();
                newSet.Add(value);
                deletions.Add(realName, newSet);
            }
            else
            {
                var deletionSet = deletions[realName];
                deletionSet.Add(value);

                deletions[realName] = deletionSet;
            }
        }
    }
}
