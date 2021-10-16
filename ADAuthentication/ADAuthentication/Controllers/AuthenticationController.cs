using ADAuthentication.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ADAuthentication.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        int DEFAULT_PORT = 0;
        string DEFAULT_DOMAIN = string.Empty;

        public IConfiguration _config { get; set; }

        public AuthenticationController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [Route("token")]
        [HttpPost]
        public HttpStatusCode Authentication([FromBody] Credentials credentials)
        {
            GetDefaultValues();
            string token = string.Empty;

            var domainName = DEFAULT_DOMAIN;

            if (ValidateUser(credentials.UserName, credentials.Password, domainName))
                return HttpStatusCode.OK;
            else
                return HttpStatusCode.NotFound;
        }

        public bool ValidateUser(string username, string password, string domainName)
        {
            if (domainName.Equals(""))
                domainName = DEFAULT_DOMAIN;

            string userDn = $"{username}@{domainName}";
            try
            {
                using (var connection = new LdapConnection { SecureSocketLayer = false })
                {
                    connection.Connect(domainName, DEFAULT_PORT);
                    connection.Bind(userDn, password);
                    if (connection.Bound)
                        return true;
                }
            }
            catch (LdapException ex)
            {
                // Log exception
            }
            return false;
        }

        public void GetDefaultValues()
        {
            DEFAULT_DOMAIN = _config.GetSection("DOMAIN_NAME").Value;
            DEFAULT_PORT = Int32.Parse(_config.GetSection("DEFAULT_PORT").Value);
        }
    }
}
