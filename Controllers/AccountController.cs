using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tigers.Data.Entities;
using Tigers.Models;

namespace Tigers.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> logger,
            SignInManager<StoreUser> signInManager,
            UserManager<StoreUser> userManager,
            IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }
        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated) //has the user already logged in
            {
                return RedirectToAction("Index", "Home"); //safety valve incase pple go to login in again, when they are already logged in
            }
            return View();
        }
        [HttpPost] //takes data from login.cshtml which when submitted will post back to the server. Need to ensure thee form has a post method
        public async Task<IActionResult> Login(LoginViewModel model) //this accepts the data we will be sent
        {
            if(ModelState.IsValid) //as there are validation rules in the loginviewmodel form
            {   //below allows user to sign in with the username and password without having to get the actual storeuser object. false re lockout on failure
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl")) //eg as in the webbrowser
                    {
                       return Redirect(Request.Query["ReturnUrl"].First()); //tells it to get the 1st value in the query string
                    }
                    else
                    {
                       return RedirectToAction("Shop", "Home"); //redirects to shop page of the app
                    }
                }
            }
            ModelState.AddModelError("", "Failed to login."); //this represents an error for the entire model
            return View(); //incase we do not login
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");  //changed from "App" to "Home"
        }
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if(ModelState.IsValid) //to see if we want to create the token
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if(user != null)
                {
                    var result = _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if(result.IsCompletedSuccessfully)
                    {
                        //create token
                        var claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"], //i.e. we who created the token
                            _config["Tokens:Audience"], //who can use this token
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(30),
                            signingCredentials: creds
                            );
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };
                        return Created("", results);
                    }
                }
                
            }
            return BadRequest(); //i.e. whether authentication didnt work, and token was not created for whatever reason is a bad request

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
