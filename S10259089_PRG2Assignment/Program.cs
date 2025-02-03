//==========================================================
// Student Number : S10259089
// Student Name : Nur Shafana Binte Mohd Saktar
// Partner Name : He Zhao Jin
//==========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using S10259089_PRG2Assignment;

namespace S10259089_PRG2Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize dictionaries
            Dictionary<string, Airline> airlines = new();
            Dictionary<string, BoardingGate> boardingGates = new();
            Dictionary<string, Flight> flights = new();

            // Load data from CSV files
            LoadAirlines(airlines);
            LoadBoardingGates(boardingGates);
            LoadFlights(flights);

            // Create Terminal object
            Terminal terminal = new Terminal(airlines, boardingGates, flights);

            // Menu system in a while loop
            while (true)
            {



                Console.Clear();
                Console.WriteLine("=============================================\r\nWelcome to Changi Airport Terminal 5\r\n=============================================");
                Console.Clear(); // Clear screen for better UI
                Console.WriteLine("========== Airport Terminal System ==========");
                Console.WriteLine("1. Assign Boarding Gates to Flights");
                Console.WriteLine("2. Create a New Flight");
                Console.WriteLine("3. Display Scheduled Flights");
                Console.WriteLine("4. Bulk Assign Unassigned Flights");
                Console.WriteLine("5. Display Total Fees Per Airline");
                Console.WriteLine("6. Reschedule a Flight");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        terminal.AssignBoardingGate();
                        break;
                    case "2":
                        Console.WriteLine("=============================================\r\nList of Boarding Gates for Changi Airport Terminal 5\r\n =============================================");
                        break;
                    case "3":
                        terminal.DisplayScheduledFlights();
                        break;
                    case "4":
                        terminal.BulkAssignBoardingGates();
                        break;
                    case "5":
                        terminal.DisplayTotalFeesPerAirline();
                        break;
                    case "6":
                        terminal.RescheduleFlight();
                        break;
                    case "7":
                        Console.WriteLine("Exiting program...");
                        return; // Exit the program

                    default:
                        Console.WriteLine(" Invalid choice. Please enter a number between 1 and 7.");
                        break;
                }

                Console.Write("\nPress any key to continue...");
                Console.ReadKey(); // Pause before displaying the menu again
            }
        }

        // Load Airlines from CSV**
        static void LoadAirlines(Dictionary<string, Airline> airlines)
        {
            string path = "airlines.csv";
            if (!File.Exists(path)) { Console.WriteLine("Airlines file not found."); return; }

            Console.WriteLine("Loading airlines...");
            var lines = File.ReadAllLines(path).Skip(1); // Skip header
            foreach (var line in lines)
            {
                string[] fields = line.Split(',').Select(f => f.Trim('\"')).ToArray();
                if (fields.Length < 2) continue;

                string code = fields[0];
                string name = fields[1];

                airlines[code] = new Airline { Code = code, Name = name };
            }
        }

        // Load Boarding Gates from CSV**
        static void LoadBoardingGates(Dictionary<string, BoardingGate> boardingGates)
        {
            string path = "boardinggates.csv";
            if (!File.Exists(path)) { Console.WriteLine("Boarding gates file not found."); return; }

            Console.WriteLine("Loading boarding gates...");
            var lines = File.ReadAllLines(path).Skip(1);
            foreach (var line in lines)
            {
                string[] fields = line.Split(',').Select(f => f.Trim('\"')).ToArray();
                if (fields.Length < 4) continue;

                boardingGates[fields[0]] = new BoardingGate
                {
                    GateName = fields[0],
                    SupportsCFFT = bool.TryParse(fields[1], out bool cfft) && cfft,
                    SupportsDDJB = bool.TryParse(fields[2], out bool ddjb) && ddjb,
                    SupportsLWTT = bool.TryParse(fields[3], out bool lwtt) && lwtt
                };
            }
        }

        // Load Flights from CSV**
        static void LoadFlights(Dictionary<string, Flight> flights)
        {
            string path = "flights.csv";
            if (!File.Exists(path)) { Console.WriteLine("Flights file not found."); return; }

            Console.WriteLine("Loading flights...");
            var lines = File.ReadAllLines(path).Skip(1);
            foreach (var line in lines)
            {
                string[] fields = line.Split(',').Select(f => f.Trim('\"')).ToArray();
                if (fields.Length < 5 || !DateTime.TryParse(fields[3], out DateTime expectedTime)) continue;

                flights[fields[0]] = new Flight
                {
                    FlightNumber = fields[0],
                    Origin = fields[1],
                    Destination = fields[2],
                    ExpectedTime = expectedTime,
                    Status = fields[4]
                };
            }
        }
    }
}
