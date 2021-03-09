using ElectronicShopManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicShopManager.Services
{
    public class DataAccessService // Used for queries that access and return , but do not update data
    {
        private readonly string strConn = "Data Source=localhost\\SQLEXPRESS01;Database=ElectronicsShopDB;Trusted_Connection=True";

        public async Task<int> Login(string username, string password) //Review Code
        {
            Users currentUser = new Users();
            using (SqlConnection sqlConn = new SqlConnection(strConn))
            {
                using (SqlCommand sqlComm = new SqlCommand("uspLogin"))
                {
                    try
                    {
                        SqlParameter usernameParam = new SqlParameter("@pLoginName", System.Data.SqlDbType.NVarChar);
                        SqlParameter passwordParam = new SqlParameter("@pPassword", System.Data.SqlDbType.NVarChar);
                        SqlParameter responseParam = new SqlParameter("@responseMessage", System.Data.SqlDbType.Int);

                        usernameParam.Value = username;
                        passwordParam.Value = password;
                        responseParam.Value = null;

                        sqlComm.Parameters.Add(usernameParam);
                        sqlComm.Parameters.Add(passwordParam);
                        sqlComm.Parameters.Add(responseParam);

                        responseParam.Direction = System.Data.ParameterDirection.ReturnValue; //parameter that is supposed to be returned as a result of a sql procedure

                        sqlComm.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlComm.Connection = sqlConn;
                        if (sqlComm.Connection.State == System.Data.ConnectionState.Closed)
                        {
                            sqlComm.Connection.Open();
                        }

                        await sqlComm.ExecuteNonQueryAsync();
                        int loginResult = (Int32)responseParam.Value; // if 0 success , if 1 - invalid password , if 2 - Invalid Username


                        ElectronicsShopDBEntities1 dbEntites = new ElectronicsShopDBEntities1();

                        if (loginResult == 0)
                        {
                            currentUser = dbEntites.Users.Where(u => u.Username == username).FirstOrDefault();
                            Application.Current.Properties["CurrentUser"] = currentUser;
                            return 0;// Successful login 

                        }
                        return loginResult; //Failed Login
                    }
                    catch 
                    {

                        sqlConn.Close();
                        return 2; 
                    }
                   
                }
            }
        }
        public async Task<List<OrderHistory>> GetUserOrdersAsync(Users currentUser) // Review Code
        {
           return await Task.Run(() => GetUserOrders(currentUser.UserID));
        }
        public  List<OrderHistory> GetUserOrders(int userID)
        {
            ElectronicsShopDBEntities1 dBEntities = new ElectronicsShopDBEntities1();

            List<OrderHistory> orders = dBEntities.OrderHistory.AsNoTracking().Where(o => o.CustomerID == userID).ToList();
            return orders;
        }
        public async Task<Dictionary<int, string>> GetCustomersAsync()
        {
            return await Task.Run(() => GetCustomers());
        }
        public Dictionary<int, string> GetCustomers()
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            var getUsersQuery = from Users in dbEntities.Users
                                    orderby Users.FirstName
                                    select new { Users.UserID, Users.FirstName, Users.LastName };
            Dictionary<int, string> wholeNames = new Dictionary<int, string>();
            foreach (var user in getUsersQuery.ToList())
            {
                string wholeName = $"{user.FirstName} {user.LastName}";
                wholeNames[user.UserID] = wholeName;
            }
            return wholeNames;
        }
        public async Task<Dictionary<int ,string>> GetEmployeesAsync()
        {
            return await Task.Run(() => GetEmployees());
        }
        public Dictionary<int, string> GetEmployees()
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            var getEmployeesQuery = from Employees in dbEntities.Employees
                                    orderby Employees.FirstName
                                    select new { Employees.EmployeeID, Employees.FirstName, Employees.LastName };
            Dictionary<int, string> wholeNames = new Dictionary<int, string>();
            foreach (var employee in getEmployeesQuery.ToList())
            {
                string wholeName = $"{employee.FirstName} {employee.LastName}";
                wholeNames[employee.EmployeeID] = wholeName;
            }
            return wholeNames;
        }
       
        public async Task<Dictionary<int ,string>> GetProductsAsync()
        {
            return await Task.Run(() => GetProducts());
        }
        public  Dictionary<int, string> GetProducts()
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            var getProductsQuery = from Products in dbEntities.Products
                                   orderby Products.ProductName
                                   select new { Products.ProductID, Products.ProductName };

            Dictionary<int, string> productNames = new Dictionary<int, string>();
            foreach (var product in getProductsQuery.ToList())
            {
                productNames[product.ProductID] = product.ProductName;
            }
            return productNames;
        }
        public int GetProductStock(int productID)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            if(productID!=-1)
            return dbEntities.Products.AsNoTracking().Where(x => x.ProductID == productID).FirstOrDefault().StockQuantity;

            return 0;
        }
       public async Task<List<OrderDetails>> ViewDetailsAsync(int OrderID)
        {
            return await Task.Run(() => ViewDetails(OrderID));
        }
        public List<OrderDetails> ViewDetails(int OrderID)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();

            List<OrderDetails> details = dbEntities.OrderDetails.AsNoTracking().Where(d => d.OrderID == OrderID).ToList();

            return details;
        }
    }
}
