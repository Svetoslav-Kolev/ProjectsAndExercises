using System;
using System.Collections.Generic;
using System.Text;

namespace ModelThis.Vehicles
{
    public class Car : MotorVehicle
    {
        public Car(double power, int maxPassenger, double maxCargoWeight) 
            : base(power, maxPassenger, maxCargoWeight)
        {

        }
    }
}
