using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADAuthentication.Interfaces
{
    public interface IAuthenticationService
    {
        bool ValidateUser(string username, string password, string domainName);
        //void GetDefaultValues();
    }
}
