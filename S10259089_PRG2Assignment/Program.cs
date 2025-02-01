//==========================================================
// Student Number : S10259089
// Student Name : Nur Shafana Binte Mohd Saktar
// Partner Name : He Zhao Jin
//==========================================================

using S10259089_PRG2Assignment;

namespace PRG2_assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            // Basic Feature 1 (Dictionaries to store airlines and boarding gates)
            Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();
            Dictionary<string, BoardingGate> boardingGates = new Dictionary<string, BoardingGate>();

            // Bsaic Feature 1 (Load airlines from CSV)
            string airlinesPath = "airlines.csv";
            if (File.Exists(airlinesPath))
            {
                Console.WriteLine("Loading airlines...");
                var airlineLines = File.ReadAllLines(airlinesPath);

                for (int i = 1; i < airlineLines.Length; i++)
                {
                    string line = airlineLines[i];
                    string[] fields = line.Split(',');

                    if (fields.Length < 2)
                    {
                        Console.WriteLine($"Skipping invalid line: {line}");
                        continue;
                    }

                    string code = fields[0];
                    string name = fields[1];

                    Airline airline = new Airline
                    {
                        Code = code,
                        Name = name
                    };

                    if (!airlines.ContainsKey(code))
                    {
                        airlines.Add(code, airline);
                    }
                }
                Console.WriteLine($"Loaded {airlines.Count} airlines.");
            }
            else
            {
                Console.WriteLine("Airlines file not found.");
            }

            // Basic Feature 1 (Load boarding gates from CSV)
            string gatesPath = "boardinggates.csv";
            if (File.Exists(gatesPath))
            {
                Console.WriteLine("Loading boarding gates...");
                var gateLines = File.ReadAllLines(gatesPath);

                for (int i = 1; i < gateLines.Length; i++)
                {
                    string line = gateLines[i];
                    string[] fields = line.Split(',');

                    if (fields.Length < 4)
                    {
                        Console.WriteLine($"Skipping invalid line: {line}");
                        continue;
                    }

                    string gateName = fields[0];
                    bool supportsCFFT = bool.Parse(fields[1]);
                    bool supportsDDJB = bool.Parse(fields[2]);
                    bool supportsLWTT = bool.Parse(fields[3]);

                    BoardingGate gate = new BoardingGate
                    {
                        GateName = gateName,
                        SupportsCFFT = supportsCFFT,
                        SupportsDDJB = supportsDDJB,
                        SupportsLWTT = supportsLWTT
                    };

                    if (!boardingGates.ContainsKey(gateName))
                    {
                        boardingGates.Add(gateName, gate);
                    }
                }
                Console.WriteLine($"Loaded {boardingGates.Count} boarding gates.");
            }
            else
            {
                Console.WriteLine("Boarding gates file not found.");
            }

            // Basic Feature 2 (Load files Flight)
            string path = "flights.csv";
            Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

            if (File.Exists(path))
            {
                Console.WriteLine("Loading flights...");

                var lines = File.ReadAllLines(path);

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] fields = line.Split(',');

                    if (fields.Length < 5)
                    {
                        Console.WriteLine($"Skipping invalid line: {line}");
                        continue;
                    }

                    string flightNumber = fields[0];
                    string origin = fields[1];
                    string destination = fields[2];

                    if (!DateTime.TryParse(fields[3], out DateTime expectedTime))
                    {
                        Console.WriteLine($"Invalid date format for line: {line}");
                        continue;
                    }

                    string status = fields[4];

                    Flight flight = new Flight
                    {
                        FlightNumber = flightNumber,
                        Origin = origin,
                        Destination = destination,
                        ExpectedTime = expectedTime,
                        Status = status
                    };

                    if (!flights.ContainsKey(flight.FlightNumber))
                    {
                        flights.Add(flight.FlightNumber, flight);
                    }
                }

                Console.WriteLine($"Successfully loaded {flights.Count} flights!");
            }
            else
            {
                Console.WriteLine("The file doesn't exist.");
            }

            //basic feature 3 (reference to Terminal.cs)
            // Create Terminal object and call the new method
            Terminal terminal = new Terminal();  
            terminal.AssignBoardingGate();  


        }

    }

}