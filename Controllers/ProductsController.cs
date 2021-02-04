using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tigers.Data;

namespace Tigers.Controllers
{
    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly ITigerRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(ITigerRepository repository, ILogger<ProductsController> logger)
        {
            _repository = repository; //repository to call and deal with data
            _logger = logger;  //log any exceptions
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_repository.GetAllProducts());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
