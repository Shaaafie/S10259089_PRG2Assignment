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
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public override double CalculateFees()
        {
            return RequestFee;
        }
        public override string ToString()
        {
            return base.ToString() + $" (LWTT, Request Fee: {RequestFee})";
        }
    }
}
