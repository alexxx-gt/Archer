using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Archer
{
    class Program
    {
        static void Main(string[] args)
        {

            MainAsync(args).Wait();
            //Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            int numberOfCommandLineArguments = args.Length;

            Handler all = new AllHandler();
            Handler cpp = new CppHandler();
            Handler reversed1 = new Reversed1Handler();
            Handler reversed2 = new Reversed2Handler();
            all.Successor = cpp;
            cpp.Successor = reversed1;
            reversed1.Successor = reversed2;

            switch (numberOfCommandLineArguments)
            {
                case 0:
                    {
                        Console.WriteLine("Not enough parameters for executing. Please, use next format: archer directory_path execution_method [output_file]");
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("Not enough parameters for executing. Please, use next format: archer directory_path execution_method [output_file]");
                        break;
                    }
                default:
                    {
                        await all.HandleRequest(args);
                        break;
                    }
            }
        }
    }

    interface IHandler
    {
        Task HandleRequest(string[] arguments);
    }

    public abstract class Handler : IHandler
    {
        const string DEFAULT_RESULTS_FILE_NAME = "results.txt";

        public List<string> fileNamesList = new List<string>();
        public Handler Successor { get; set; }
        public async virtual Task HandleRequest(string[] arguments)
        {
            if (arguments.Length == 3)
            {
                Console.WriteLine(arguments.Length);
                await WriteToFile(arguments[0], arguments[2]);

            }
            else
            {
                Console.WriteLine(arguments.Length);
                await WriteToFile(arguments[0]);
            }
        }

        public string ReversePath(string item)
        {
            string[] path = item.Split("\\".ToCharArray());
            Array.Reverse(path);
            string res = string.Empty;
            for (int i = 0; i < path.Length; i++)
            {
                res = res + path[i];
                if (i != path.Length - 1)
                {
                    res = res + "\\";
                }
            }

            return res;
        }

        public string ReverseString(string item)
        {
            char[] itemAsChararray = item.ToCharArray();
            Array.Reverse(itemAsChararray);
            return new string(itemAsChararray);
        }

        public async Task GetSubdirectoryFiles(string directory, string filter = "")
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            DirectoryInfo[] subfolders = di.GetDirectories();
            FileInfo[] files = di.GetFiles();

            if (subfolders.Length > 0)
            {
                foreach (DirectoryInfo item in subfolders)
                {
                    await Task.Run(async () =>
                    {
                        await GetSubdirectoryFiles(item.FullName, filter);
                    });
                }
            }

            foreach (FileInfo item in files)
            {
                if (item.FullName.EndsWith(filter))
                {
                    fileNamesList.Add(item.FullName);
                }
            }

        }

        public async Task WriteToFile(string directory, string resultFileName = DEFAULT_RESULTS_FILE_NAME)
        {
            string path = directory + "\\" + resultFileName;

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (string item in fileNamesList)
                {
                    await writer.WriteLineAsync(item);
                }
            }
        }

        public void RemoveInitialFolderName(string folderName)
        {
            for (int i = 0; i < fileNamesList.Count; i++)
            {
                string element = fileNamesList.ElementAt(i);
                element = element.Replace(folderName + "\\", string.Empty);
                fileNamesList.RemoveAt(i);
                fileNamesList.Insert(i, element);
            }
        }
    }

    public class AllHandler : Handler
    {

        public async override Task HandleRequest(string[] arguments)
        {
            if (arguments[1] == "all")
            {
                await GetSubdirectoryFiles(arguments[0]);
                RemoveInitialFolderName(arguments[0]);

                await base.HandleRequest(arguments);
            }
            else
            {
                await Successor.HandleRequest(arguments);
            }
        }
    }

    public class CppHandler : Handler
    {
        public async override Task HandleRequest(string[] arguments)
        {
            if (arguments[1] == "cpp")
            {
                await GetSubdirectoryFiles(arguments[0], ".cpp");
                RemoveInitialFolderName(arguments[0]);

                await base.HandleRequest(arguments);
            }
            else
            {
                await Successor.HandleRequest(arguments);
            }
        }
    }

    public class Reversed1Handler : Handler
    {
        public async override Task HandleRequest(string[] arguments)
        {
            if (arguments[1] == "reverse1")
            {
                await GetSubdirectoryFiles(arguments[0]);
                RemoveInitialFolderName(arguments[0]);

                for (int i = 0; i < fileNamesList.Count; i++)
                {
                    string element = fileNamesList.ElementAt(i);
                    element = ReversePath(element);
                    fileNamesList.RemoveAt(i);
                    fileNamesList.Insert(i, element);
                }

                await base.HandleRequest(arguments);
            }
            else
            {
                await Successor.HandleRequest(arguments);
            }
        }
    }

    public class Reversed2Handler : Handler
    {
        public async override Task HandleRequest(string[] arguments)
        {
            if (arguments[1] == "reverse2")
            {
                await GetSubdirectoryFiles(arguments[0]);
                RemoveInitialFolderName(arguments[0]);

                for (int i = 0; i < fileNamesList.Count; i++)
                {
                    string element = fileNamesList.ElementAt(i);
                    element = ReverseString(element);
                    fileNamesList.RemoveAt(i);
                    fileNamesList.Insert(i, element);
                }

                await base.HandleRequest(arguments);
            }
            else
            {
                await Successor.HandleRequest(arguments);
            }
        }
    }
}

