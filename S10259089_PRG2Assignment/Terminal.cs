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
