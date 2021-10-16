using ADAuthentication.Interfaces;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADAuthentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        int DEFAULT_PORT = 0;
        string DEFAULT_DOMAIN = string.Empty;
        
        public IConfiguration _config { get; set; }
        
        public AuthenticationService(IConfiguration configuration)
        {
            _config = configuration;
            GetDefaultValues();
        }

        public bool ValidateUser(string username, string password, string domainName = "")
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
            DEFAULT_DOMAIN = _config.GetSection("DEFAULT_DOMAIN").Value;
            DEFAULT_PORT = Int32.Parse(_config.GetSection("DEFAULT_PORT").Value);
        }
    }
}
