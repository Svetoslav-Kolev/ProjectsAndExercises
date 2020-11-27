using Kursova.Models;
using Kursova.Models.Context;
using Kursova.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<ShopController> _logger;
        OutfitsContext dbContext;

        public ShopController(ILogger<ShopController> logger, OutfitsContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;

        }
        public IActionResult Outfits()
        {
            OutfitsViewModel outfitsModel = new OutfitsViewModel();
            outfitsModel.AllOutfits = dbContext.Outfits.ToList();
            return View(outfitsModel);
        }
        [HttpGet]
        public IActionResult AddOutfit()
        {
            AddOutfitModel viewModel = new AddOutfitModel();
            viewModel.AllClothes = dbContext.Clothing.ToList();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult AddOutfit(AddOutfitModel addoutfitModel)
        {
            Outfits outfit = new Outfits();
            outfit.OutfitName = addoutfitModel.OutfitName;
            Clothing pieceOne = dbContext.Clothing.Where(c => c.ClothingID == addoutfitModel.pieceOneId).FirstOrDefault();
            Clothing pieceTwo = dbContext.Clothing.Where(c => c.ClothingID == addoutfitModel.pieceTwoId).FirstOrDefault();
            Clothing pieceThree = dbContext.Clothing.Where(c => c.ClothingID == addoutfitModel.pieceThreeId).FirstOrDefault();
            
            outfit.Clothes.Add(pieceOne);
            outfit.Clothes.Add(pieceTwo);
            outfit.Clothes.Add(pieceThree);
         
            pieceOne.Outfits.Add(outfit);

            pieceTwo.Outfits.Add(outfit);

            pieceThree.Outfits.Add(outfit);

            dbContext.Outfits.Add(outfit);

            dbContext.SaveChanges();

            return RedirectToAction("Outfits");
        }
        [HttpPost]
        public IActionResult Order(ShopViewModel shopViewModel)
        {
            string currUserEmail = HttpContext.Session.GetString("User");
            User currUser = dbContext.Users.Where(u => u.email == currUserEmail).FirstOrDefault();
            OrderViewModel orderModel = new OrderViewModel();

            orderModel.ClothingID = shopViewModel.selectedClothingID;

            orderModel.yourCards =dbContext.Users.Where(u => u.UserID == currUser.UserID).SelectMany(u => u.CreditCards).ToList();

            return View(orderModel);
        }
        [HttpPost]
        public IActionResult OrderClothing(OrderViewModel orderModel)
        {

            string currUserEmail = HttpContext.Session.GetString("User");
            User currUser = dbContext.Users.Where(u => u.email == currUserEmail).FirstOrDefault();

            Clothing clothingToOrder = dbContext.Clothing.Where(c => c.ClothingID == orderModel.ClothingID).FirstOrDefault();

            Order order = new Order();
            order.Date = DateTime.Now;
            order.User = currUser;
            order.DeliveryAddress = orderModel.DeliveryAddress;
          

            OrderDetails orderDetails = new OrderDetails();
            orderDetails.Quantity = orderModel.Quantity;
            orderDetails.Price = clothingToOrder.Price;
            orderDetails.Order = order;
            orderDetails.Clothing = clothingToOrder;

            order.TotalPrice = orderDetails.Quantity * orderDetails.Price;

            Transaction transaction = new Transaction();
            transaction.Order = order;
            transaction.TransactionType = orderModel.TransactionType;
            if (orderModel.TransactionType == "card")
            {
                transaction.CreditCard = dbContext.CreditCards.Where(c => c.CreditCardID == orderModel.CreditCardId).FirstOrDefault();
               
            }
          

            dbContext.Orders.Add(order);
            dbContext.OrderDetails.Add(orderDetails);
            dbContext.Transactions.Add(transaction);
            dbContext.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Shop()
        {
            ShopViewModel shopModel = new ShopViewModel();
            bool isAuth = HttpContext.Session.GetString("Authenticated") == "True";
            ViewData["User"] = HttpContext.Session.GetString("User");

            if (!isAuth)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (Clothing piece in dbContext.Clothing)
            {
                shopModel.AllClothing.Add(piece);
            }

            return View(shopModel);
        }
        [HttpGet]
        public IActionResult AddClothes()
        {
            ClothingViewModel clothingModel = new ClothingViewModel();
            return View(clothingModel);
        }
        [HttpPost]
        public IActionResult AddClothes(ClothingViewModel clothingModel)
        {
            Clothing clothing = new Clothing();

            clothing.ClothingName = clothingModel.ClothingName;
            clothing.Material = clothingModel.Material;
            clothing.Price = clothingModel.Price;
            clothing.Color = clothingModel.Color;

            if (!dbContext.Category.Any(c => c.CategoryName == clothingModel.Category))
            {
                Category category = new Category();
                category.CategoryName = clothingModel.Category;
                dbContext.Category.Add(category);
                dbContext.SaveChanges();
            }

            clothing.Category = dbContext.Category.Where(c => c.CategoryName == clothingModel.Category).FirstOrDefault();
            dbContext.Clothing.Add(clothing);

            dbContext.SaveChanges();

            return RedirectToAction("Shop");
        }

    }
}
