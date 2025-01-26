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
        //implement 1 
        static void main(String[] args)
        {
            string path = "fight.csv";
            StreamReader reader = null;

            //create dictionary to store the flights
            Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

            //check if file exists
            if (File.Exists(path))
            {
                Console.WriteLine("Loading flights");

                //read all lines in the file
                var lines = File.ReadAllLines(path);

                for (int i = 1; i < lines.Length; i++)
                {
                    String LINE = lines[i];
                    string[] fields = LINE.Split(' ');
                    //create flight object
                    string flightNumber = fields[0];
                    string origin = fields[1];
                    string destination = fields[2];
                    DateTime expectedTime = DateTime.ParseExact(fields[3]);
                        string status = fields[4];

                    //create and populate the flight object
                    Flight flight = new Flight
                    {
                        FlightNumber = flightNumber,
                        origin = origin,
                        destination = destination,
                        expectedTime = expectedTime,
                        status = status

                    };
                    //add flight object to the dictionary
                    if (!flights.ContainsKey(flight.FlightNumber))
                    {
                        flights.Add(flight.FlightNumber, flight);
                    }
                }

                Console.WriteLine($"Sucessfully loaded{flights.Count} flights! ");
            }
            else
            {
                Console.WriteLine("The file dosen't exist");
            }
        }
    }
}