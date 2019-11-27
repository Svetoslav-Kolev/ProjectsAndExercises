using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElevatorProject
{
   public class Elevator
    {
        private Semaphore semaphore;
        public Floor CurrFloor { get; set; }
        public Floor TargetFloor { get; set; }
        public Agent CurrAgent { get; set; }
        //public Queue<ElevatorCall> ElevatorCalls { get; set; }
        public Elevator()
        {
            this.semaphore = new Semaphore(1, 1);
            this.CurrFloor = Floor.Ground;
          //  this.ElevatorCalls = new Queue<ElevatorCall>();
        }
        public async Task Call(ElevatorCall currCall)
        {
            
            //ElevatorCalls.Enqueue(currCall);
            semaphore.WaitOne();
            //while (ElevatorCalls.Peek().Agent.Id != currCall.Agent.Id)
            //{
            //   await ProcessQueue(currCall);
             
            //}
            
            await ProcessQueue(currCall);
          

        }

        private async Task ProcessQueue(ElevatorCall currCall)
        {
            
            //Console.WriteLine($"{currCall.Agent.Id} - first element");

                await Task.Run(()=>ReceiveCall(currCall));
            //ElevatorCalls.Dequeue();
            
        }

        private void ReceiveCall(ElevatorCall currCall)
        {
            
            this.CurrAgent = currCall.Agent;
            int waitTime = Math.Abs((int)CurrFloor - (int)currCall.TargetFloor);
            Console.WriteLine($"Elevator called by agent {CurrAgent.Id} from {(int)this.CurrFloor} floor to {(int)currCall.TargetFloor} floor.");
            Thread.Sleep(1000 * waitTime);
            Console.WriteLine(waitTime + " second passed.");
            this.CurrFloor = currCall.TargetFloor;

        
            
            int targetFloor =  currCall.Agent.ChooseFloor();
        
            int waitTimeSecond = Math.Abs((int)CurrFloor - targetFloor);
            Console.WriteLine($"Elevator went for {targetFloor} floor. requested by agent {CurrAgent.Id}");
            Thread.Sleep(1000 * waitTimeSecond);
            this.CurrFloor = (Floor)targetFloor;

            while ((int)CurrFloor > CurrAgent.SecurityClearenceLevel)
            {
                Console.WriteLine($"Agent {CurrAgent.Id} security clearence is not high enough , choosing another floor!");
                targetFloor =  CurrAgent.ChooseFloor();
                Console.WriteLine($"Elevator went for {targetFloor} floor. requested by agent {CurrAgent.Id}");
                waitTimeSecond = Math.Abs((int)CurrFloor - targetFloor);
                Thread.Sleep(1000 * waitTimeSecond);
                Console.WriteLine($"{waitTime} seconds passed");
                this.CurrFloor = (Floor)targetFloor;

            }

            Console.WriteLine($"Elevator reached {(int)CurrFloor} floor after {waitTimeSecond} seconds.");

            semaphore.Release();
        }
        
    }
}
