using Dapper;
using ElectronicShopManager.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManager.Services
{
   
    public class DataUpdateService // Used for any queries that actually update the database
    {
        public ConnectionStringSettings settings =
             ConfigurationManager.ConnectionStrings["ElectronicsShopDBEntitiesAddedByHand"];
        private string strConn;
        public DataUpdateService()
        {
            strConn = settings.ConnectionString;
        }
        public async Task ModifyOrderAsync(OrderHistory modifiedOrder)
        {
            await Task.Run(() => ModifyOrder(modifiedOrder));
        }
        public void ModifyOrder(OrderHistory modifiedOrder)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            OrderHistory orderToModify = dbEntities.OrderHistory.Where(o => o.OrderID == modifiedOrder.OrderID).FirstOrDefault();

            orderToModify.EmployeeID = modifiedOrder.EmployeeID;
            orderToModify.DeliveryAddress = modifiedOrder.DeliveryAddress;
            orderToModify.ReceiptNumber = modifiedOrder.ReceiptNumber;
            orderToModify.Status = modifiedOrder.Status;
            orderToModify.OrderDate = modifiedOrder.OrderDate;

            dbEntities.SaveChanges();           
        }
        public async Task DeleteOrderDetailAsync(int detailID)
        {
            await Task.Run(() => DeleteOrderDetail(detailID));
        }
        public void DeleteOrderDetail(int detailID)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            OrderDetails detailsToRemove = dbEntities.OrderDetails.Where(d => d.OrderDetailID == detailID).FirstOrDefault();
            dbEntities.OrderDetails.Remove(detailsToRemove);
            dbEntities.SaveChanges(); //this has to be saved because the price of the details is computed in the database 
            decimal newPrice = GetNewPrice(detailsToRemove.OrderID); 
            dbEntities.OrderHistory.Where(o => o.OrderID == detailsToRemove.OrderID).FirstOrDefault().TotalPrice = newPrice;
            dbEntities.SaveChanges();
        }
        public async Task<OrderDetails> AddOrderDetailAsync(OrderDetails details)
        {
           return await Task.Run(() => AddOrderDetail(details));
        }
        public OrderDetails AddOrderDetail(OrderDetails details)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();


            OrderDetails detailsToAdd = new OrderDetails();
            detailsToAdd.OrderID = details.OrderID;
            detailsToAdd.ProductID = details.ProductID;
            detailsToAdd.Quantity = details.Quantity;
            detailsToAdd.UnitPrice = GetProductPrice(detailsToAdd.ProductID);
            detailsToAdd.Discount = details.Discount;

            dbEntities.OrderDetails.Add(detailsToAdd);

            dbEntities.SaveChanges(); //this has to be saved because the price of the details is computed in the database 
            decimal newPrice = GetNewPrice(details.OrderID);
            dbEntities.OrderHistory.Where(x => x.OrderID == details.OrderID).FirstOrDefault().TotalPrice = newPrice;
            dbEntities.SaveChanges();
            return detailsToAdd;
        }
        public async Task AddOrderAsync(Users currentUser, OrderHistory order, OrderDetails details)
        {
            await Task.Run(() => AddOrder(currentUser, order, details));
        }
        public void AddOrder(Users currentUser, OrderHistory order, OrderDetails details)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();

            OrderHistory newOrder = new OrderHistory();
            newOrder.OrderDate = DateTime.Now;
            newOrder.ReceiptNumber = GetRandomString(13);
            newOrder.EmployeeID = order.EmployeeID;
            newOrder.CustomerID = currentUser.UserID;
            newOrder.DeliveryAddress = order.DeliveryAddress;
            newOrder.Status = order.Status;
            newOrder.CurrencyCode = "USD";

            OrderDetails initialDetails = new OrderDetails();
            initialDetails.OrderID = newOrder.OrderID;
            initialDetails.ProductID = details.ProductID;
            initialDetails.Quantity = details.Quantity;
            initialDetails.UnitPrice = GetProductPrice(initialDetails.ProductID);
            initialDetails.Discount = details.Discount;

            newOrder.OrderDetails.Add(initialDetails);
            dbEntities.OrderHistory.Add(newOrder);
            dbEntities.OrderDetails.Add(initialDetails);

            dbEntities.SaveChanges();
            // Add total price after automatically calculating it in DB 
            decimal newPrice = GetNewPrice(newOrder.OrderID);
            newOrder.TotalPrice = newPrice; //still tracked
            dbEntities.SaveChanges();
        }
        private decimal GetNewPrice(int orderID) //Gets the collective price of the order from all of its details
        {
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                decimal newPrice = 0;
                var dapperDetails = connection.Query("Select [PriceWithDiscount] From OrderDetails Where [OrderDetails].[OrderID] = @orderID", new { orderID = orderID }).ToList();
                foreach (var detail  in dapperDetails)
                {
                    newPrice += detail.PriceWithDiscount;
                }
                return newPrice;
            }

            //ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            //List<OrderDetails> allDetails = dbEntities.OrderDetails.AsNoTracking().Where(x => x.OrderID == orderID).ToList();
            //decimal newPrice = 0;
            //foreach (var detail in allDetails)
            //{
            //    newPrice += (decimal)detail.PriceWithDiscount;
            //}
            //return newPrice;
        }
        private decimal GetProductPrice(int productID)
        {           
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                var dapperProduct = connection.Query("Select [Price] From Products Where [Products].[ProductID] = @productID", new { productID = productID }).ToList().FirstOrDefault();
                return dapperProduct.Price;
            }

            //ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();
            //decimal price = dbEntities.Products.AsNoTracking().Where(p => p.ProductID == productID).FirstOrDefault().Price;


            //return price;
        }
        private string GetRandomString(int length) //Generates an example receipt number  
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
     .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task DeleteOrderAsync(int orderID)
        {
            await Task.Run(() => DeleteOrder(orderID));
        }
        public void DeleteOrder(int OrderID)
        {
            ElectronicsShopDBEntities1 dbEntities = new ElectronicsShopDBEntities1();

            OrderHistory orderToRemove = dbEntities.OrderHistory.Where(x => x.OrderID == OrderID).FirstOrDefault();

            dbEntities.OrderDetails.RemoveRange(orderToRemove.OrderDetails.ToList());

            dbEntities.OrderHistory.Remove(orderToRemove);
            dbEntities.SaveChanges();
        }
    }
}
