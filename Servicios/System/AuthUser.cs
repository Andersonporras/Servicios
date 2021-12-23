using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;

namespace Servicios
{
    public class AuthUser : SoapHeader
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (this.UserName == "admin" && this.Password == "123")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}