using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace ElevatorProject
{
    class StartUp
    {
        
        static Elevator elevator = new Elevator();
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            Agent[] agents = new Agent[10];
            Task[] tasks = new Task[10];
            for (int i = 0; i < agents.Length; i++)
            {
                int clearence = rnd.Next(1, 5);
                agents[i] = new Agent(clearence, (Floor)rnd.Next(1, clearence), i); // Floor might not be parsed to int
            }
            int counter = 0;
            foreach (var agent in agents)
            {
          
                tasks[counter] = (Task.Factory.StartNew(()=>ControlAgent(agent)));
              
                counter++;
            }
            Task.WaitAll(tasks);

        }
        static async Task ControlAgent(Agent agent)
        {
            int random = rnd.Next(2, 5);
            int counter = 0;
            while (counter<random) 
            {
                Thread.Sleep(rnd.Next(1,4) * 1000);
                Console.WriteLine("             Agent " + agent.Id + " called the elevator.");
              
                ElevatorCall ec = agent.CallElevator();
                await elevator.Call(ec);
                counter++;
            }
            Console.WriteLine("             Agent " + agent.Id + " went home.");


        }
       
    
    }
}
