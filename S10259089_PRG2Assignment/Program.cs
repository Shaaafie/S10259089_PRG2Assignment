//==========================================================
// Student Number : S10259089
// Student Name : Nur Shafana Binte Mohd Saktar
// Partner Name : He Zhao Jin
//==========================================================

using System;
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
            // Basic Feature 1: Dictionaries to store airlines and boarding gates
            Dictionary<string, Airline> airlines = new();
            Dictionary<string, BoardingGate> boardingGates = new();

            // Load airlines from CSV
            string airlinesPath = "airlines.csv";
            if (File.Exists(airlinesPath))
            {
                Console.WriteLine("Loading airlines...");
                var airlineLines = File.ReadAllLines(airlinesPath);

                for (int i = 1; i < airlineLines.Length; i++)
                {
                    string[] fields = airlineLines[i].Split(',').Select(f => f.Trim('\"')).ToArray();

                    if (fields.Length < 2)
                    {
                        Console.WriteLine($"Skipping invalid line: {airlineLines[i]}");
                        continue;
                    }

                    string code = fields[0];
                    string name = fields[1];

                    if (!airlines.ContainsKey(code))
                    {
                        airlines[code] = new Airline { Code = code, Name = name };
                    }
                }
                Console.WriteLine($"Loaded {airlines.Count} airlines.");
            }
            else
            {
                Console.WriteLine("Airlines file not found.");
            }

            // Load boarding gates from CSV
            string gatesPath = "boardinggates.csv";
            if (File.Exists(gatesPath))
            {
                Console.WriteLine("Loading boarding gates...");
                var gateLines = File.ReadAllLines(gatesPath);

                for (int i = 1; i < gateLines.Length; i++)
                {
                    string[] fields = gateLines[i].Split(',').Select(f => f.Trim('\"')).ToArray();

                    if (fields.Length < 4)
                    {
                        Console.WriteLine($"Skipping invalid line: {gateLines[i]}");
                        continue;
                    }

                    if (!bool.TryParse(fields[1], out bool supportsCFFT)) supportsCFFT = false;
                    if (!bool.TryParse(fields[2], out bool supportsDDJB)) supportsDDJB = false;
                    if (!bool.TryParse(fields[3], out bool supportsLWTT)) supportsLWTT = false;

                    string gateName = fields[0];

                    if (!boardingGates.ContainsKey(gateName))
                    {
                        boardingGates[gateName] = new BoardingGate
                        {
                            GateName = gateName,
                            SupportsCFFT = supportsCFFT,
                            SupportsDDJB = supportsDDJB,
                            SupportsLWTT = supportsLWTT
                        };
                    }
                }
                Console.WriteLine($"Loaded {boardingGates.Count} boarding gates.");
            }
            else
            {
                Console.WriteLine("Boarding gates file not found.");
            }

            // Load flights from CSV
            string flightsPath = "flights.csv";
            Dictionary<string, Flight> flights = new();

            if (File.Exists(flightsPath))
            {
                Console.WriteLine("Loading flights...");
                var lines = File.ReadAllLines(flightsPath);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',').Select(f => f.Trim('\"')).ToArray();

                    if (fields.Length < 5)
                    {
                        Console.WriteLine($"Skipping invalid line: {lines[i]}");
                        continue;
                    }

                    if (!DateTime.TryParse(fields[3], out DateTime expectedTime))
                    {
                        Console.WriteLine($"Invalid date format for line: {lines[i]}");
                        continue;
                    }

                    string flightNumber = fields[0];

                    if (!flights.ContainsKey(flightNumber))
                    {
                        flights[flightNumber] = new Flight
                        {
                            FlightNumber = flightNumber,
                            Origin = fields[1],
                            Destination = fields[2],
                            ExpectedTime = expectedTime,
                            Status = fields[4]
                        };
                    }
                }
                Console.WriteLine($"Successfully loaded {flights.Count} flights!");
            }
            else
            {
                Console.WriteLine("Flights file not found.");
            }

            // Create Terminal object and call the new method
            Terminal terminal = new Terminal(airlines, boardingGates, flights);

            // Basic Feature 3: Assign Boarding Gates to Flights
            terminal.AssignBoardingGate();

            // Basic Feature 6: Create a New Flight
            terminal.CreateNewFlight(flights, flightsPath);
        }
    }
}
