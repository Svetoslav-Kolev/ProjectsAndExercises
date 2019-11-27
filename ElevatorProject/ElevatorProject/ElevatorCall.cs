using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProject
{
    public class ElevatorCall
    {
        public Agent Agent { get; set; }
        public Floor TargetFloor { get; set; }
        public ElevatorCall(Agent agent,Floor trgtFloor)
        {
            this.Agent = agent;
            this.TargetFloor = trgtFloor;
        }
    }
}
