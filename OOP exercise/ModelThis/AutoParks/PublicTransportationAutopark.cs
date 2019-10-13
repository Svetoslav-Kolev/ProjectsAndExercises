using System;
using System.Collections.Generic;
using System.Text;

namespace ModelThis.AutoParks
{
    public class PublicTransportationAutopark : StandartAutoPark
    {
        private int maxPassengers;

        public int MaxPassengers
        {
            get { return maxPassengers; }
           private set { maxPassengers = value; }
        }

        public PublicTransportationAutopark(int maxPassengers):base()
        {
            this.MaxPassengers = maxPassengers;
        }
    }
}
