using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestJobFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the total no of processes:");
            if (!int.TryParse(Console.ReadLine(), out int num))
            {
                Console.WriteLine("Invalid number please try again.");
                Main(args);
            }

            List<ProcessModel> processList = new List<ProcessModel>();
            for (int i = 0; i < num; i++)
            {
                processList.Add(GetProcessInfo());
            }
            
            RunProcesses(processList);
            PrintResult(processList);
        }

        // counter represents time flow and starts with 0
        static int counter = 0;
        
        // runs the list of processes according to shortest job first algorithm
        static void RunProcesses(List<ProcessModel> processList)
        {
            if (!processList.Where(p => p.P_Arrival <= counter).Any())
            {
                counter++;
                RunProcesses(processList);
            }

            var processes = processList.Where(p => p.P_Arrival <= counter && p.P_Bust > 0);
            if (processes.Any())
            {
                var shortest = processList.Where(p => p.P_Arrival <= counter && p.P_Bust > 0).OrderBy(p => p.P_Bust).FirstOrDefault();

                shortest.P_Bust = shortest.P_Bust - 1;
                counter++;
                if (shortest.P_Bust == 0)
                {
                    //Console.WriteLine("Waiting Time for " + shortest.P_Name + "is " + (counter - shortest.P_Arrival - shortest.First_P_Bust));
                    shortest.WaitingTime = counter - shortest.P_Arrival - shortest.First_P_Bust;
                }
                RunProcesses(processList);
            }
            else
            {
                return;
            }
        }

        // Prints the result as a readable table view
        static void PrintResult(List<ProcessModel> processList)
        {
            var consoleTable = new ConsoleTable("Process Name", "Arrival Time", "Burst Time", "Waiting Time");
            foreach (var process in processList.OrderBy(p => p.P_Arrival))
            {
                consoleTable.AddRow(process.P_Name, process.P_Arrival, process.First_P_Bust, process.WaitingTime);
            }
            consoleTable.Write(Format.Alternative);
            Console.WriteLine();

            var totalWaiting = processList.Sum(p => p.WaitingTime);
            var averageWaiting = processList.Average(p => p.WaitingTime);
            Console.WriteLine("Total waiting time: " + totalWaiting);
            Console.WriteLine("Average waiting time: " + averageWaiting);
            Console.ReadLine();
        }

        // takes the information of each process from the user.
        public static ProcessModel GetProcessInfo()
        {
            ProcessModel processModel = new ProcessModel();

            Console.WriteLine("Enter p_name:");
            processModel.P_Name = Console.ReadLine();

            processModel.P_Arrival = GetIntVal("Enter p_arrival:");
            int bust = GetIntVal("Enter p_bust:");
            processModel.P_Bust = bust;
            processModel.First_P_Bust = bust;
            
            return processModel;
        }

        // takes input entered by user and check if it is valid integer. 
        // if input is not valid for integer, prints error message to trigger user to reenter the input.
        public static int GetIntVal(string message)
        {
            Console.WriteLine(message);
            if (!int.TryParse(Console.ReadLine(), out int num))
            {
                Console.WriteLine("Invalid number please try again.");
                return GetIntVal(message);
            }
            return num;
        }
    }

    
    public class ProcessModel
    {
        public string P_Name { get; set; }
        public int P_Arrival { get; set; }
        public int P_Bust { get; set; }
        public int First_P_Bust { get; set; }
        public int WaitingTime { get; set; }
    }
}
