using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10259089_PRG2Assignment
{
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCTTF { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }
        public double CalculateFees()
        {
            return 300;
        }
        public string ToString()
        {
            return "Gate: " + GateName +
                "Flight: " + Flight != null ? Flight.FlightNumber : "None";
        }
    }
}
