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
