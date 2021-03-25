using CompanyGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Testing
{
    class TestingShortestPath
    {
        CompanyGraph.CompanyGraph companyGraph;

        Company VIK = new Company() { ID = 1, Name = "VIK" };
        Company Kodar = new Company() { ID = 2, Name = "Kodar" };
        Company SBTech = new Company() { ID = 3, Name = "SB-Tech" };
        Company MentorMate = new Company() { ID = 4, Name = "MentorMate" };
        Company Zdravkova = new Company() { ID = 5, Name = "Zdravkova" };
        Company Non = new Company() { ID = 6, Name = "Non" };
        Company Tesla = new Company() { ID = 7, Name = "Tesla" };
        Company Unconnected = new Company { ID = 8, Name = "Unconnected" };

        [Test]
        public void ShortestPathInLinearGraph()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { Kodar });
            companyGraph.Add(Kodar, new List<Company> { SBTech });
            companyGraph.Add(SBTech, new List<Company> { MentorMate });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });

            companyGraph.Add(Unconnected, new List<Company> { });
            List<string> shortestPath = new List<string> { VIK.Name, Kodar.Name, SBTech.Name, MentorMate.Name, Zdravkova.Name, Non.Name };
            
            Assert.AreEqual(companyGraph.FindShortestPath(Non,VIK), shortestPath);
        }
       
        [Test]
        public void ShortestPathInBranchedGraph()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { Kodar, SBTech, MentorMate });
            companyGraph.Add(SBTech, new List<Company> { Zdravkova, });
            companyGraph.Add(SBTech, new List<Company> { Tesla });
            companyGraph.Add(Tesla, new List<Company> { Non });
            companyGraph.Add(Kodar, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });
            companyGraph.Add(MentorMate, new List<Company> { Non });


            companyGraph.FindShortestPath(VIK, Non);

            companyGraph.Add(Unconnected, new List<Company> { });
            List<string> shortestPath = new List<string> { Non.Name,MentorMate.Name,VIK.Name};

            Assert.AreEqual(companyGraph.FindShortestPath(VIK, Non), shortestPath);
        }
        [Test]
        public void ManyCompaniesToOneFromDifferentConnections()
        {
            companyGraph = new CompanyGraph.CompanyGraph();


            companyGraph.Add(VIK, new List<Company> { Kodar, SBTech, MentorMate });
            companyGraph.Add(Kodar, new List<Company> { Zdravkova });
            companyGraph.Add(SBTech, new List<Company> { Zdravkova });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });
            companyGraph.Add(MentorMate, new List<Company> { Non });

            List<string> shortestPath = new List<string> { Zdravkova.Name, MentorMate.Name, VIK.Name };
            List<string> shortestPathToNon = new List<string> { Non.Name, MentorMate.Name, VIK.Name };

            Assert.AreEqual(companyGraph.FindShortestPath(VIK, Zdravkova), shortestPath);
            Assert.AreEqual(companyGraph.FindShortestPath(VIK, Non), shortestPathToNon);
        }
        [Test]
        public void ShortestPathIsCorrectAfterRemovingACompany()
        {
            companyGraph = new CompanyGraph.CompanyGraph();


            companyGraph.Add(VIK, new List<Company> { Kodar, SBTech, MentorMate });
            companyGraph.Add(Kodar, new List<Company> { Zdravkova });
            companyGraph.Add(SBTech, new List<Company> { Zdravkova });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });
            companyGraph.Add(MentorMate, new List<Company> { Non });
            companyGraph.Remove(MentorMate);
            List<string> shortestPath = new List<string> { Zdravkova.Name, SBTech.Name, VIK.Name };
            List<string> shortestPathToNon = new List<string> { Non.Name, Zdravkova.Name,SBTech.Name, VIK.Name };

            Assert.AreEqual(companyGraph.FindShortestPath(VIK, Zdravkova), shortestPath);
            Assert.AreEqual(companyGraph.FindShortestPath(VIK, Non), shortestPathToNon);
        }

    }
}
