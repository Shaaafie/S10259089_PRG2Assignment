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
            string path = "flight.csv";
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
        }
    }

    public class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Flight {FlightNumber} from {Origin} to {Destination} at {ExpectedTime}. Status: {Status}";
        }
    }
}