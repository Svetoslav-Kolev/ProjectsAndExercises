using CompanyGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;


namespace Testing
{
    class Tests
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
        public void BranchedConnections()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { Kodar });
            companyGraph.Add(VIK, new List<Company> { SBTech });
            companyGraph.Add(VIK, new List<Company> { MentorMate });

            companyGraph.Add(SBTech, new List<Company> { Zdravkova });
            companyGraph.Add(SBTech, new List<Company> { Non });
            companyGraph.Add(SBTech, new List<Company> { Tesla });

            companyGraph.Add(Unconnected, new List<Company> { });

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(MentorMate, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(MentorMate, Zdravkova), true);

            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(Unconnected, Zdravkova), false);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(Unconnected, Zdravkova), false);
        }
        [Test]
        public void LinearConnections()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { Kodar });
            companyGraph.Add(Kodar, new List<Company> { SBTech });
            companyGraph.Add(SBTech, new List<Company> { MentorMate });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });

            companyGraph.Add(Unconnected, new List<Company> {  });

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(Non, VIK), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(Non, VIK), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(Zdravkova, Kodar), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(Zdravkova, Kodar), true);

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(Zdravkova, Unconnected), false);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(Zdravkova, Unconnected), false);
        }
        [Test]
        public void RemovingACompany()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { Kodar });
            companyGraph.Add(Kodar, new List<Company> { SBTech });
            companyGraph.Add(SBTech, new List<Company> { MentorMate });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });
            companyGraph.Remove(SBTech);
            companyGraph.Add(Unconnected, new List<Company> { });

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Non), false);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Non), false);

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(MentorMate, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(MentorMate, Non), true);

        }
        [Test]
        public void SingleCompanyInGraph()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> {  });
       

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, VIK), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, VIK), true);


        }
        [Test]
        public void MixedConnections()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { Kodar, SBTech });
            companyGraph.Add(SBTech, new List<Company> { MentorMate, VIK });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Non, new List<Company> { Zdravkova });
            

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, MentorMate), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(MentorMate, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(MentorMate, Zdravkova), true);
        }
        [Test]
        public void InGraphButNoConnection()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { });
            companyGraph.Add(SBTech, new List<Company> { });

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, SBTech), false);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, SBTech), false);
        }
        [Test]
        public void OneCompanyNotInGraph()
        {
            companyGraph = new CompanyGraph.CompanyGraph();

            companyGraph.Add(VIK, new List<Company> { });

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, SBTech), false);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, SBTech), false);
        }
        [Test]
        public void BothCompaniesNotInGraph()
        {
            companyGraph = new CompanyGraph.CompanyGraph();
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, SBTech), false);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, SBTech), false);
        }
        [Test]
        public void ManyCompaniesToOneFromDifferentConnections()
        {
            companyGraph = new CompanyGraph.CompanyGraph();


            companyGraph.Add(VIK, new List<Company> { Kodar,SBTech,MentorMate });
            companyGraph.Add(Kodar, new List<Company> { Zdravkova });
            companyGraph.Add(SBTech, new List<Company> { Zdravkova });
            companyGraph.Add(MentorMate, new List<Company> { Zdravkova });
            companyGraph.Add(Zdravkova, new List<Company> { Non });
            companyGraph.Add(MentorMate, new List<Company> { Non });

            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Zdravkova), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(VIK, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedDepthFirst(Kodar, Non), true);
            Assert.AreEqual(companyGraph.AreConnectedBreadthFirst(Kodar, Non), true);
        }
    }
}
