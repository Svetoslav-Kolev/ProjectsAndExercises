using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGraph
{
    public class CompanyGraph //Graph is not directed  || adjacency list representation
    {
        private Dictionary<int, List<Company>> Companies { get; set; } // companyID is the key and the value is a list of connected companies

        public CompanyGraph()
        {
            Companies = new Dictionary<int, List<Company>>();

        }
        /// <summary>
        /// If the company already exists in the graph, it only adds new links, 
        /// If company does not exist in the graph it adds the company and creates the new links
        /// </summary>
        public void Add(Company initialCompany, List<Company> links)
        {
            if (!Companies.ContainsKey(initialCompany.ID)) //if initial company does not exist , create it and link it to all designated companies
            {
                Companies.Add(initialCompany.ID, links);
            }
            foreach (var company in links)
            {
                //if initial company exists but does not contain a link to linked company , create one
                if (!Companies[initialCompany.ID].Contains(company)) 
                    Companies[initialCompany.ID].Add(company);

                // if linked company does not exist , create it with a link to initialCompany
                if (!Companies.ContainsKey(company.ID))
                {
                    Companies.Add(company.ID, new List<Company> { initialCompany });
                }
                else
                {
                    //if linked company exists but does not contain a link to initialCompany , create one
                    if (!Companies[company.ID].Contains(initialCompany))
                        Companies[company.ID].Add(initialCompany);
                }
                                 
            }
        }
        public void Remove(Company company)
        {
            if (Companies.ContainsKey(company.ID))
            {
                Companies.Remove(company.ID);
                foreach (var listLinks in Companies.Values)
                {
                    if (listLinks.Contains(company))
                        listLinks.Remove(company);
                }
                Console.WriteLine($"{company.Name} removed!");
            }
            else
            {
                Console.WriteLine("Company not in graph!");
            }
           
        }

        public bool AreConnectedDepthFirst(Company companyOne, Company companyTwo) // don't want to expose the list parameter
        {
            if (Companies.ContainsKey(companyOne.ID) == false || Companies.ContainsKey(companyTwo.ID) == false)
            {
                return false;
            }
            return AreConnectedDepthFirst(companyOne, companyTwo, new List<int> { });
        }
        private bool AreConnectedDepthFirst(Company companyOne, Company companyTwo, List<int> visitedCompanies)  // Depth First
        {
            visitedCompanies.Add(companyOne.ID);

            if (companyOne == companyTwo)
            {
                return true;
            }
            else
            {
                foreach (var connectedCompany in Companies[companyOne.ID])
                {
                    bool hasPath = false;
                    if (!visitedCompanies.Contains(connectedCompany.ID))
                    {
                        hasPath = AreConnectedDepthFirst(connectedCompany, companyTwo, visitedCompanies);
                        if (hasPath)
                        {
                            return true;
                        }
                    }

                }
            }

            return false;
        }
        public bool AreConnectedBreadthFirst(Company companyOne, Company companyTwo) //Breadth First
        {
            if (Companies.ContainsKey(companyOne.ID) == false || Companies.ContainsKey(companyTwo.ID) == false)
            {
                return false;
            }
            Dictionary<int,bool> VisitedCompanies = new Dictionary<int, bool>();
            Queue<Company> queue = new Queue<Company>();
            queue.Enqueue(companyOne);
            VisitedCompanies.Add(companyOne.ID, true);
            while (queue.Count > 0)
            {              
                Company currentCompany = queue.Dequeue();                       
                if (currentCompany == companyTwo)
                {
                    return true;
                }
                foreach (var linkedCompany in Companies[currentCompany.ID])
                {
                    if (!VisitedCompanies.ContainsKey(linkedCompany.ID))
                    {
                        VisitedCompanies.Add(linkedCompany.ID, true);
                        queue.Enqueue(linkedCompany);
                    }
                       
                }
            }
            return false;
        }
        /// <summary>
        /// Dijkstra algorithm for shortest path 
        /// </summary>
        public List<string> FindShortestPath(Company sourceCompany, Company TargetCompany)//BFS search is faster when edges have no weight (such as my case) , this is made for practice and experimentation
        {
            if (AreConnectedBreadthFirst(sourceCompany, TargetCompany) && sourceCompany != TargetCompany)
            {
                Dictionary<string, string> PredecessorDictionary = new Dictionary<string, string>(); // value is predecessor
                Dictionary<Company, int> ShortestPathSet = new Dictionary<Company, int>(); //Serves as a priority queue , value is distance
                fillSet(sourceCompany, ShortestPathSet);

                while (ShortestPathSet.Count > 0)
                {
                    Company closest = ShortestPathSet.Aggregate((l, r) => l.Value < r.Value ? l : r).Key; // finds element with smallest value
                    int closestDistance = ShortestPathSet[closest];
                    ShortestPathSet.Remove(closest);

                    if (closest == TargetCompany)   
                    {
                        break;
                    }
                    foreach (var connectedCompany in Companies[closest.ID])
                    {
                        if (ShortestPathSet.ContainsKey(connectedCompany) && ShortestPathSet[connectedCompany] > closestDistance + 1)
                        {
                            ShortestPathSet[connectedCompany] = closestDistance + 1;
                            PredecessorDictionary.Add(connectedCompany.Name, closest.Name);
                        }

                    }



                }
                //Path is printed out from target to source

                List<string> finalPath = new List<string>();
                finalPath.Add(TargetCompany.Name); //target company
                string predecessor = PredecessorDictionary[TargetCompany.Name];
                while (predecessor != sourceCompany.Name) //everything in between
                {
                    finalPath.Add(predecessor);
                    predecessor = PredecessorDictionary[predecessor];
                }
                finalPath.Add(predecessor); //source company

                return finalPath;
            }
            else
            {
                Console.WriteLine("Companies are not connected");
                return null;
            }

        }
        private void fillSet(Company sourceCompany, Dictionary<Company, int> shortestPathSet) //BFS but fills initial distance values for the Dijkstra  algorithm
        {
            Dictionary<int,bool> VisitedCompanies = new Dictionary<int, bool>();
            VisitedCompanies.Add(sourceCompany.ID,true);
            Queue<Company> queue = new Queue<Company>();
            shortestPathSet[sourceCompany] = 0;
            foreach (var connectedCompany in Companies[sourceCompany.ID])
            {
                VisitedCompanies.Add(connectedCompany.ID, true);
                queue.Enqueue(connectedCompany);
                
            }

            while (queue.Count > 0)
            {
                Company currentCompany = queue.Dequeue();
                
                shortestPathSet[currentCompany] = int.MaxValue;
                foreach (var connectedCompany in Companies[currentCompany.ID])
                {
                    if (!VisitedCompanies.ContainsKey(connectedCompany.ID))
                    {
                        VisitedCompanies.Add(connectedCompany.ID, true);
                        queue.Enqueue(connectedCompany);
                    }
                        
                }
            }
        }
    }
}
