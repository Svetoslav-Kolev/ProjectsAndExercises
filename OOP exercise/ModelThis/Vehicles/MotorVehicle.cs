using System;
using System.Collections.Generic;
using System.Text;
using ModelThis.Contracts;
namespace ModelThis.Vehicles
{
    public abstract class MotorVehicle : Vehicle
    {
        private double power;
        private int maximumPassengers;
        private double maxCargoWeight;
        public double Power
        {
            get { return power; }
            private set { power = value; }
        }

        public int MaximumPassengers { get => maximumPassengers; private set => maximumPassengers = value; }
        public double MaxCargoWeight { get => maxCargoWeight; private set => maxCargoWeight = value; }

        public MotorVehicle(double power, int maxPassenger, double maxCargoWeight)
        {
            this.Power = power;
            this.MaximumPassengers = maxPassenger;
            this.MaxCargoWeight = maxCargoWeight;
        }
    }
}
