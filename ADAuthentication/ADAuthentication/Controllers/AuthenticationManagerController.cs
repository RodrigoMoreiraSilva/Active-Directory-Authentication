using ADAuthentication.Interfaces;
using ADAuthentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationManagerController : ControllerBase
    {
        public IAuthenticationService _authenticationService { get; set; }

        [Route("api/token")]
        [HttpPost]
        public string GetToken([FromBody]string user, string password)
        {
            string token = string.Empty;

            var domainName = "";

            _authenticationService.ValidateUser(domainName, user, password);


            return token;
        }
    }
}
