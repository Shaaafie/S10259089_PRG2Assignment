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
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }
        public double CalculateFees()
        {
            return 300;
        }
        public string ToString()
        {
            return $"GateName: {GateName}, " +
               $"CFFT: {SupportsCFFT}, " +
               $"DDJB: {SupportsDDJB}, " +
               $"LWTT: {SupportsLWTT}, " +
               $"Flight: {(Flight != null ? Flight.ToString() : "No Flight Assigned")}";
        }
    }
}
