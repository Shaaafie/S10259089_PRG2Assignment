//==========================================================
// Student Number : S10259089
// Student Name : Nur Shafana Binte Mohd Saktar
// Partner Name : He Zhao Jin
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10259089_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new();
        public Dictionary<string, Flight> Flights { get; set; } = new();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new();
        public Dictionary<string, double> GateFees { get; set; } = new();
        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Code))
            {
                Airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }
        public bool AddBoardingGate(BoardingGate gate)
        {
            if (BoardingGates.ContainsKey(gate.GateName))
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

        //feature 3 : Assign a boarding gate to a flight
        public void AssignBoardingGate()
        {
            // Prompt for Flight Number
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine();

            // Check if Flight exists in the dictionary
            if (!Flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Flight not found. Please enter a valid Flight Number.");
                return;
            }

            Flight flight = Flights[flightNumber]; // Retrieve Flight object
            Console.WriteLine($"Selected Flight: {flight.FlightNumber}, Destination: {flight.Destination}");

            // Prompt for Boarding Gate
            Console.Write("Enter Boarding Gate: ");
            string gateName = Console.ReadLine();

            // Check if Boarding Gate exists
            if (!BoardingGates.ContainsKey(gateName))
            {
                Console.WriteLine("Boarding Gate not found. Please enter a valid Gate.");
                return;
            }

            BoardingGate gate = BoardingGates[gateName]; // Retrieve Boarding Gate object

            // Check if the Gate is already assigned to another Flight
            if (gate.Flight != null)
            {
                Console.WriteLine("This gate is already assigned to another flight. Please choose another gate.");
                return;
            }

            // Assign the Flight to the Gate
            gate.Flight = flight;
            Console.WriteLine($"Successfully assigned {flight.FlightNumber} to gate {gate.GateName}!");

            // Prompt to update Flight Status
            Console.Write("Would you like to update the Flight Status? (Y/N): ");
            string updateStatus = Console.ReadLine().Trim().ToUpper();

            if (updateStatus == "Y")
            {
                Console.WriteLine("Enter new Status (Delayed/Boarding/On Time): ");
                string status = Console.ReadLine();
                flight.Status = status;
                Console.WriteLine($"Flight {flight.FlightNumber} status updated to {flight.Status}.");
            }
        }

        //basic featue 5
        public void AssignBoardingGate(Dictionary<string, Flight> flights, Dictionary<string, BoardingGate> boardingGates)
        {

            Console.Write("Enter the flight number: ");
            string flightNumber = Console.ReadLine().Trim();

        
            if (!flights.ContainsKey(flightNumber))
            {
                Console.WriteLine("Flight not found.");
                return;
            }


            Flight flight = flights[flightNumber];
            Console.WriteLine($"Flight Details: {flight}");


            Console.Write("Enter the boarding gate: ");
            string gateName = Console.ReadLine().Trim();


            if (!boardingGates.ContainsKey(gateName))
            {
                Console.WriteLine("Boarding gate not found.");
                return;
            }

            BoardingGate gate = boardingGates[gateName];

   
            if (gate.Flight != null)
            {
                Console.WriteLine("This boarding gate is already assigned to another flight.");
                return;
            }

            gate.Flight = flight;
            Console.WriteLine($"Boarding gate {gateName} assigned to flight {flightNumber}.");


            Console.Write("Do you want to update the flight status? (Y/N): ");
            string updateStatus = Console.ReadLine().Trim().ToUpper();

            if (updateStatus == "Y")
            {
                Console.Write("Enter the new status (Delayed, Boarding, On Time): ");
                string newStatus = Console.ReadLine().Trim();
                flight.Status = newStatus;
                Console.WriteLine($"Flight status updated to {newStatus}.");
            }
            else
            {
                flight.Status = "On Time"; 
            }

            Console.WriteLine("Boarding gate assignment completed successfully.");
        }

        //basic feature 6:create a new flight
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
                            newFlight = new DDJBFlight { RequestFee = 300 }; 
                            break;
                        case "CFFT":
                            newFlight = new CFFTFlight { RequestFee = 150 };
                            break;
                        case "LWTT":
                            newFlight = new LWTTFlight { RequestFee = 500 }; 
                            break;
                        default:
                            Console.WriteLine("Invalid special request code. Creating a normal flight.");
                            newFlight = new NORMFlight();
                            break;
                    }
                }
                else
                {
                    newFlight = new NORMFlight();
                }

           
                newFlight.FlightNumber = flightNumber;
                newFlight.Origin = origin;
                newFlight.Destination = destination;
                newFlight.ExpectedTime = expectedTime;
                newFlight.Status = "On Time"; 

                flights.Add(flightNumber, newFlight);

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



        public override string ToString()
        {
            return "Terminal: " + TerminalName +
                "Airlines: " + Airlines +
                "Flights: " + Flights +
                "BoardingGates: " + BoardingGates +
                "GateFees: " + GateFees;
        }
    }
}
