using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using szakdolgozatAPI_v02.Data;
using szakdolgozatAPI_v02.Models;
using szakdolgozatAPI_v02.ViewModels;

namespace szakdolgozatAPI_v02.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;
        private readonly IParkingContext dbContext;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, IParkingContext dbContext)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        [Route("add-plates")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddLicensePlates(string[] licensePlates)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            foreach (var licensePlate in licensePlates)
            {
                await this.dbContext.LicensePlates.AddAsync(new LicensePlate()
                {
                    CreationDate = DateTime.Now,
                    LicensePlateText = licensePlate.Replace("-", string.Empty).Replace(" ", string.Empty).ToUpper(),
                    UserID = userId
                });
            }
            if (await this.dbContext.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            return BadRequest();
           
        }
        [Route("get-user")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserData()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await this.userManager.FindByIdAsync(userId);
            var userVM = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = await this.userManager.IsInRoleAsync(user, "Admin"),
                LicensePlates = new List<string>()
            };

            foreach (var licensePlate in user.LicensePlates)
            {
                userVM.LicensePlates.Add(licensePlate.LicensePlateText);
            }

            return Ok(userVM);
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser(RegisterViewModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                LicensePlates = new List<LicensePlate>(),
                SecurityStamp = Guid.NewGuid().ToString()
            };
            


            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
            }

            foreach (var item in model.LicensePlates)
            {
                var licensePlate = new LicensePlate
                {
                    User = user,
                    LicensePlateText = item.Replace("-", "").ToUpper(),
                    CreationDate = DateTime.Now
                };
                await dbContext.LicensePlates.AddAsync(licensePlate);
                await dbContext.SaveChangesAsync();
            }

            return Ok( user);
        }


        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            
            var user = await userManager.FindByEmailAsync(model.Email);
            
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName.ToString())
                };
                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SigningKey"]));

                bool isAdmin = (await userManager.GetRolesAsync(user)).Contains("Admin");
                var token = new JwtSecurityToken(
                        issuer: configuration["Jwt:Site"],
                        audience: configuration["Jwt:Site"],
                        claims: new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName.ToString())
                        },
                        expires: DateTime.Now.AddDays(2),
                        signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature));
               

                return Ok(
                    new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        admin = isAdmin
                    });
            }
            return Unauthorized("Hibás felhasználónév vagy jelszó.");
        }
    }
}
