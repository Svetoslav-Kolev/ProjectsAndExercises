using System;
using System.Collections.Generic;
using System.Text;

namespace ModelThis.Vehicles
{
    public class Van : MotorVehicle
    {
        public Van(double power, int maxPassenger, double maxCargoWeight) 
            : base(power, maxPassenger, maxCargoWeight)
        {
        }
    }
}
