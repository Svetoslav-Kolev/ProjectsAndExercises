using Kursova.Models;
using Kursova.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursova.Controllers
{
    public class AccountController:Controller
    {
        private readonly ILogger<AccountController> _logger;
        OutfitsContext dbContext;
        
        public AccountController(ILogger<AccountController> logger, OutfitsContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
          
        }
        [HttpPost]
         public IActionResult AddCard(AddCardViewModel cardModel)
        {
            string currUserEmail = HttpContext.Session.GetString("User");
            User currUser = dbContext.Users.Where(u => u.email == currUserEmail).FirstOrDefault();
            CreditCard card = new CreditCard();

            card.CardNumber = cardModel.CardNumber;
            card.CardType = cardModel.CardType;
            card.ExpiryMonth = cardModel.ExpiryMonth;
            card.ExpiryYear = cardModel.ExpiryYear;
            card.Users = new List<User>();

            card.Users.Add(currUser);
            if(currUser.CreditCards == null)
            {
                currUser.CreditCards = new List<CreditCard>();
            }
            currUser.CreditCards.Add(card);

            dbContext.SaveChanges();

            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult AddCard()
        {
            AddCardViewModel cardModel = new AddCardViewModel();
            return View(cardModel);
        }
        public IActionResult AllCards()
        {
            string currUserEmail = HttpContext.Session.GetString("User");
          
            AllCardsViewModel model = new AllCardsViewModel();

            User currUser = dbContext.Users.Where(u => u.email == currUserEmail).FirstOrDefault();
            var allCardsOfUser = dbContext.Users.Where(u => u.UserID == currUser.UserID).SelectMany(u => u.CreditCards).ToList();
            if (allCardsOfUser == null)
            {
                ViewData["CreditCards"] = "You have not added any cards";
            }
            else
            {
                model.userCards = allCardsOfUser;
            }
            return View(model);
        }
        public IActionResult OrderHistory()
        {
            string currUserEmail = HttpContext.Session.GetString("User");
            User currUser = dbContext.Users.Where(u => u.email == currUserEmail).FirstOrDefault();

            OrderHistoryViewModel orderHistory = new OrderHistoryViewModel();
            orderHistory.myOrders = dbContext.Orders.Where(o => o.UserID == currUser.UserID).ToList();

            return View(orderHistory);
        }
    }
}
