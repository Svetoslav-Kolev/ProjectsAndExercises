using System;
using System.Collections.Generic;
using System.Text;

namespace ModelThis.Vehicles
{
    public class Bus : MotorVehicle
    {
        public Bus(double power, int maxPassenger, double maxCargoWeight) 
            : base(power, maxPassenger, maxCargoWeight)
        {
        }
    }
}
