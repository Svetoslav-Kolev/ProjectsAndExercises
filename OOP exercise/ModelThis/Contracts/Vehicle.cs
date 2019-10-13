using System;
using System.Collections.Generic;
using System.Text;

namespace ModelThis.Contracts
{
   public interface Vehicle
    {
         double Power { get; }
        int MaximumPassengers { get; }
        double MaxCargoWeight { get; }

    }
}
