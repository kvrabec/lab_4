using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using LAB4.MODEL.Entities.ActuallyUsefullClasses;
using LAB4.MODEL.Entities;
using Microsoft.Extensions.Configuration;

namespace LAB_4.Controllers
{
    [Produces("application/json")]
    [Route("api/Token")]
    public class TokenController : Controller
    {
        private readonly StoreDbContext _context;
        public IConfiguration Configuration { get; }

        public TokenController(StoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpPost("RequestToken")]
        public IActionResult RequestToken([FromBody] TokenRequestModel tokenRequest)
        {
            if (_context.Customer.Any(c => c.Person.FirstName == tokenRequest.FirstName
        && c.AccountNumber == tokenRequest.AccountNumber))
            {
                JwtSecurityToken token = JwsTokenCreator.CreateToken(tokenRequest.FirstName,
            Configuration["Auth:JwtSecurityKey"],
            Configuration["Auth:ValidIssuer"],
            Configuration["Auth:ValidAudience"]);
                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(tokenStr);
            }
            return Unauthorized();
        }

    }
}
