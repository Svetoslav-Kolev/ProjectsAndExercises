using System;
using System.Collections.Generic;
using System.Text;
using ModelThis.Contracts;
using ModelThis.Vehicles;

namespace ModelThis.AutoParks
{
    public class LogisticsAutopark : StandartAutoPark
    {
        private double maxCargoWeight;

        public double MaxCargoWeight
        {
            get { return maxCargoWeight; }
           private set { maxCargoWeight = value; }
        }

        public LogisticsAutopark(double maxCargoWeight):base()
        {
            this.MaxCargoWeight = maxCargoWeight;
        }
    }
}
