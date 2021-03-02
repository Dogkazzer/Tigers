using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tigers.Data;
using Tigers.Data.Entities;


namespace Tigers
{
    public class TigerSeeder
    {
        private readonly TigerContext _ctx;
        private readonly IWebHostEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        [Obsolete]
        public TigerSeeder(TigerContext ctx, IWebHostEnvironment hosting, UserManager<StoreUser> userManager)
        {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }
        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();
            StoreUser user = await _userManager.FindByEmailAsync("hil@tigers.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FristName = "Hil",
                    LastName = "W",
                    Email = "hil@tigers.com",
                    UserName = "hil@tigers.com"
                };
                var result = await _userManager.CreateAsync(user, "PX9XW@@rd");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in Seeder.");
                }
            }



                if (!_ctx.Products.Any())
            { 
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if(order != null)
                {
                    order.User = user;  //this updates the order to be owned by the user
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product=products.First(),
                            Quantity=5,
                            UnitPrice=products.First().Price
                        }
                    };
                }
                _ctx.SaveChanges();
            }
        }
    }
}
