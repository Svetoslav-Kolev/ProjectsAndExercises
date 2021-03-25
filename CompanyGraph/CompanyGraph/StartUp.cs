using System;
using System.Collections.Generic;

namespace CompanyGraph
{
    class StartUp
    {
        static void Main(string[] args)
        {
            Company VIK = new Company() { ID=1,Name= "VIK"};
            Company Kodar = new Company() { ID = 2, Name = "Kodar" };
            Company SBTech = new Company() { ID = 3, Name = "SB-Tech" };
            Company MentorMate = new Company() { ID = 4, Name = "MentorMate" };
            Company Zdravkova = new Company() { ID = 5, Name = "Zdravkova" };
            Company Non = new Company() { ID = 6, Name = "Non" };
            Company Tesla = new Company() { ID = 7, Name = "Tesla" };
            Company Unconnected = new Company { ID = 8, Name = "Unconnected" };
            CompanyGraph companyGraph = new CompanyGraph();

            

            companyGraph.Add(VIK, new List<Company> { Kodar, SBTech, MentorMate });
            companyGraph.Add(SBTech, new List<Company> { Zdravkova, });
            companyGraph.Add(SBTech, new List<Company> { Tesla });
            companyGraph.Add(Tesla, new List<Company> { Non });
            companyGraph.Add(Kodar, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });
            companyGraph.Add(MentorMate, new List<Company> { Non });

            Console.WriteLine();
            companyGraph.FindShortestPath(VIK, VIK);

            companyGraph.Add(Unconnected, new List<Company> { });
            List<string> shortestPath = new List<string> { Non.Name, MentorMate.Name, VIK.Name };

            


        }
    }
}
