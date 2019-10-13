using System;
using System.Collections.Generic;
using System.Text;
using ModelThis.Vehicles;

namespace ModelThis.Contracts
{
   public interface AutoPark
    {
        List<MotorVehicle> Vehicles { get; }
    }
}
