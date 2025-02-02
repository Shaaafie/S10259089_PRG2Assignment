//==========================================================
// Student Number : S10259089
// Student Name : Nur Shafana Binte Mohd Saktar
// Partner Name : He Zhao Jin
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;

namespace S10259089_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new();
        public Dictionary<string, Flight> Flights { get; set; } = new();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new();
        public Dictionary<string, double> GateFees { get; set; } = new();

        // New Dictionary to track flight assignments
        public Dictionary<string, string> FlightAssignments { get; set; } = new();

        public Terminal(Dictionary<string, Airline> airlines, Dictionary<string, BoardingGate> boardingGates, Dictionary<string, Flight> flights)
        {
            Airlines = airlines;
            BoardingGates = boardingGates;
            Flights = flights;
        }

        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates[gate.GateName] = gate;
                return true;
            }
            return false;
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                    return airline;
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines.Values)
            {
                double totalFees = airline.CalculateTotalFees();
                double totalDiscounts = 0;

                foreach (var flight in airline.Flights.Values)
                {
                    totalDiscounts += airline.CalculateDiscounts(flight);
                }

                Console.WriteLine($"{airline.Name}: Total Fees = {totalFees:C}, Discounts = {totalDiscounts:C}, Final Bill = {totalFees - totalDiscounts:C}");
            }
        }

        // Feature 3: Assign a boarding gate to a flight
        public void AssignBoardingGate()
        {
            foreach (var flight in Flights.Values)
            {
                foreach (var gate in BoardingGates.Values)
                {
                    if (gate.Flight == null)  // Only assign an empty gate
                    {
                        // Ensure the gate supports the flight type
                        if (flight is CFFTFlight && !gate.SupportsCFFT) continue;
                        if (flight is DDJBFlight && !gate.SupportsDDJB) continue;
                        if (flight is LWTTFlight && !gate.SupportsLWTT) continue;

                        gate.Flight = flight;
                        FlightAssignments[flight.FlightNumber] = gate.GateName;  // ✅ Store in dictionary
                        break;  // Stop once a gate is assigned
                    }
                }
            }
        }

        // Feature 5: Assign a flight manually
        public void AssignFlightManually(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates, string flightsPath)
        {
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine();

            if (flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Flight already exists.");
                return;
            }

            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine();

            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();

            Console.Write("Enter Expected DateTime (yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime expectedTime))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Console.Write("Enter Status: ");
            string status = Console.ReadLine();

            Flight newFlight = new Flight
            {
                FlightNumber = flightNumber,
                Origin = origin,
                Destination = destination,
                ExpectedTime = expectedTime,
                Status = status
            };

            flights[flightNumber] = newFlight;
            Console.WriteLine($"Successfully added flight {flightNumber}!");

            File.AppendAllText(flightsPath, $"{flightNumber},{origin},{destination},{expectedTime},{status}\n");
        }

        // Feature 6: Create a new flight
        public void CreateNewFlight(Dictionary<string, Flight> flights, string flightsPath)
        {
            while (true)
            {
                Console.Write("Enter flight number: ");
                string flightNumber = Console.ReadLine().Trim();

                if (flights.ContainsKey(flightNumber))
                {
                    Console.WriteLine("Flight already exists.");
                    continue;
                }

                Console.Write("Enter origin: ");
                string origin = Console.ReadLine().Trim();

                Console.Write("Enter destination: ");
                string destination = Console.ReadLine().Trim();

                Console.Write("Enter expected departure/arrival time (yyyy-MM-dd HH:mm): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime expectedTime))
                {
                    Console.WriteLine("Invalid date format.");
                    continue;
                }

                Console.Write("Do you want to enter a special request code? (Y/N): ");
                string addSpecialRequest = Console.ReadLine().Trim().ToUpper();

                Flight newFlight;

                if (addSpecialRequest == "Y")
                {
                    Console.Write("Enter special request code (DDJB, CFFT, LWTT): ");
                    string specialRequestCode = Console.ReadLine().Trim().ToUpper();

                    switch (specialRequestCode)
                    {
                        case "DDJB":
                            newFlight = new DDJBFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time", RequestFee = 300 };
                            break;
                        case "CFFT":
                            newFlight = new CFFTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time", RequestFee = 150 };
                            break;
                        case "LWTT":
                            newFlight = new LWTTFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time", RequestFee = 500 };
                            break;
                        default:
                            Console.WriteLine("Invalid special request code. Creating a normal flight.");
                            newFlight = new NORMFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time" };
                            break;
                    }
                }
                else
                {
                    newFlight = new NORMFlight { FlightNumber = flightNumber, Origin = origin, Destination = destination, ExpectedTime = expectedTime, Status = "On Time" };
                }

                flights[flightNumber] = newFlight;

                string newFlightLine = $"{flightNumber},{origin},{destination},{expectedTime:yyyy-MM-dd HH:mm},{newFlight.Status}";
                File.AppendAllText(flightsPath, Environment.NewLine + newFlightLine);

                Console.WriteLine("New flight created and added successfully.");

                Console.Write("Do you want to add another flight? (Y/N): ");
                string addAnother = Console.ReadLine().Trim().ToUpper();

                if (addAnother != "Y")
                {
                    break;
                }
            }

            
        }
        // Feature 9: Display Scheduled Flights in Chronological Order
        public void DisplayScheduledFlights()
        {
            // Sort the flights by ExpectedTime using IComparable implementation
            var sortedFlights = Flights.Values.OrderBy(flight => flight).ToList();

            Console.WriteLine("Scheduled Flights for Today:");

            foreach (var flight in sortedFlights)
            {
                // Find the airline associated with the flight
                var airline = GetAirlineFromFlight(flight);
                string airlineName = airline != null ? airline.Name : "Unknown Airline";

                // Check if the flight has an assigned boarding gate
                string gateAssignment = "Not Assigned";
                foreach (var gate in BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        gateAssignment = gate.GateName;
                        break;
                    }
                }

                // Display the flight information along with airline name, special request code, and boarding gate assignment
                Console.WriteLine($"Flight {flight.FlightNumber}: {airlineName}");
                Console.WriteLine($"Origin: {flight.Origin}, Destination: {flight.Destination}");
                Console.WriteLine($"Scheduled Time: {flight.ExpectedTime:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Status: {flight.Status}");
                Console.WriteLine($"Boarding Gate: {gateAssignment}");
                Console.WriteLine();
            }
        }


        public override string ToString()
        {
            return $"Terminal: {TerminalName}\n" +
                   $"Airlines: {string.Join(", ", Airlines.Values.Select(a => a.Name))}\n" +
                   $"Flights: {string.Join(", ", Flights.Values.Select(f => f.FlightNumber))}\n" +
                   $"BoardingGates: {string.Join(", ", BoardingGates.Values.Select(g => g.GateName))}\n" +
                   $"GateFees: {string.Join(", ", GateFees.Select(g => $"{g.Key}: {g.Value:C}"))}";
        }
    }
}

