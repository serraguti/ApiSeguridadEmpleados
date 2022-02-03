using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridadEmpleados.Helpers
{
    public class HelperOAuthToken
    {
        public String Issuer { get; set; }
        public String Audience { get; set; }
        public String SecretKey { get; set; }

        public HelperOAuthToken(IConfiguration configuration)
        {

        }
    }
}
