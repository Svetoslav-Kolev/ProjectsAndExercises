using System;
using System.Collections.Generic;
using System.Text;
using ModelThis.Contracts;
using ModelThis.Vehicles;

namespace ModelThis.AutoParks
{
    public abstract  class StandartAutoPark : AutoPark
    {
        private List<MotorVehicle> vehicles;
        public List<MotorVehicle> Vehicles { get => vehicles; set => vehicles = value; }

        public StandartAutoPark()
        {
            this.Vehicles = new List<MotorVehicle>();
        }
    }
}
