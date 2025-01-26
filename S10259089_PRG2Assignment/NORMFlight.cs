using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10259089_PRG2Assignment
{
    public class NORMFlight : Flight
    {
        public override double CalculateFees()
        {
            return 0;
        }
        public override string ToString()
        {
            return base.ToString() + " (NORM)";
        }
    }
}
