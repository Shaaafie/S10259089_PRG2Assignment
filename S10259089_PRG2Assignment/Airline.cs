using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace S10259089_PRG2Assignment
{
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public bool AddFlight(Flight flight)
        {
            return false;
        }
        public double CalculateTotalFees() 
        {
            return Flights.Values.Sum(f => f.CalculateFees());
        }
        public double CalculateDiscounts(Flight flight)
        {
            double discount = 0;
            int flightCount = Flights.Count;
            discount += (flightCount / 3) * 350;
            foreach (var flights in Flights.Values)
            {
                if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                {
                    discount += 110;
                }
                if (new[] { "DXB", "BKK", "NRT" }.Contains(flight.Origin))
                {
                    discount += 25;
                }
                if (!(flight is CFFTFlight || flight is LWTTFlight || flight is DDJBFlight))
                {
                    discount += 50;
                }
            }
            if (flightCount > 5)
            {
                discount += 0.03 * CalculateTotalFees();
            }
            return discount;
        }
        public string ToString()
        {
            return "Name: " + Name +
                "Code: " + Code;
        }
    }
}
