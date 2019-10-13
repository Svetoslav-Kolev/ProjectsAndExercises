using System;
using ModelThis.Vehicles;
using ModelThis.AutoParks;
using System.Linq;

namespace ModelThis
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            // Example Code for the main method
            LogisticsAutopark logisticsAutopark = new LogisticsAutopark(20);
            PublicTransportationAutopark publicTransportation = new PublicTransportationAutopark(25);

            MotorVehicle[] vehicles = new MotorVehicle[] 
            {
                new Van(10, 15, 20),
                new Bus(10,15,20),
                new Car(5,10,10)

            };

            publicTransportation.Vehicles = vehicles.ToList();
            logisticsAutopark.Vehicles = vehicles.ToList();

            StandartAutoPark[] autoParks = new StandartAutoPark[]
            {
                publicTransportation,
                logisticsAutopark
            };

            foreach (var autopark in autoParks)
            {
               if(autopark is LogisticsAutopark logautopark)
                {
                    Console.WriteLine($"Logistics autopark - max cargo weight: {logautopark.MaxCargoWeight} and number of vehicles {logautopark.Vehicles.Count}");
                }
                else if (autopark is PublicTransportationAutopark publicAutopark)
                {
                    Console.WriteLine($"Public Transport autopark - max passengers: {publicAutopark.MaxPassengers} and number of vehicles {publicAutopark.Vehicles.Count}");
                }
                else
                {
                    Console.WriteLine($"Standart Autopark - max passengers: {autopark.Vehicles.Count}");
                }
            }
        }
    }
}
