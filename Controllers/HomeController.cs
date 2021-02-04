using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tigers.Data;
using Tigers.Models;

namespace Tigers.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        private readonly ITigerRepository _repository;
        //private readonly IMailService _mailService;

        public HomeController(ILogger<HomeController> logger, ITigerRepository repository) /*IMailService mailService*/
        {
            _logger = logger;
            _repository = repository;
            //_mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]  //this necessitates anyone who wants to buy anything to be logged inor have some credential
        public IActionResult Shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }
        //[HttpGet("contact")]
        //public IActionResult Contact()
        //{
        //    return View();
        //}

        //[HttpPost("contact")]
        //public IActionResult Contact(ContactViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Send the email
        //        _mailService.SendMessage("shawn@wildermuth.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
        //        ViewBag.UserMessage = "Mail Sent";
        //        ModelState.Clear();
        //    }

        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
