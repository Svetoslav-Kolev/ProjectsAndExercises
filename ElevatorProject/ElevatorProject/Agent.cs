using System;
using System.Collections.Generic;
using System.Text;

namespace ElevatorProject
{
   public class Agent
    {
        public int SecurityClearenceLevel { get; set; }
        public int Id { get; set; }
        Floor currFloor { get; set; }
      
        public Agent(int clearance, Floor startingFloor,int id)
        {
            this.Id = id;
            this.currFloor = startingFloor;
            this.SecurityClearenceLevel = clearance;
        }
        public ElevatorCall CallElevator()
        {
            return new ElevatorCall(this, currFloor);  
        }
        public  int ChooseFloor()
        {
            Random rnd = new Random();
            return rnd.Next(1, 5);

        }
    }
}
