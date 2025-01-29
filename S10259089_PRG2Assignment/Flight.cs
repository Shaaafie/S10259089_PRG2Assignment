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
    public class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        public virtual double CalculateFees()
        {
            double fee = 0;
            if (Destination == "SIN")
            {
                fee += 500;
            }
            else if (Origin == "SIN")
            {
                fee += 800;
            }
            return fee;
        }
        public override string ToString()
        {
            return "FlightNumber: " + FlightNumber +
                "Origin: " + Origin +
                "Destination: " + Destination +
                "ExpectedTime: " + ExpectedTime +
                "Status: " + Status;
        }
    }
}
