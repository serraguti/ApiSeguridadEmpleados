using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this.Issuer = configuration.GetValue<string>("ApiOAuth:Issuer");
            this.Audience = configuration.GetValue<string>("ApiOAuth:Audience");
            this.SecretKey = configuration.GetValue<string>("ApiOAuth:SecretKey");
        }

        //NECESITAMOS UN METODO PARA GENERAR UNA CLAVE A PARTIR
        //DE NUESTRO SECRET KEY
        public SymmetricSecurityKey GetKeyToken()
        {
            //CONVERTIMOS A BYTE EL SECRET KEY
            byte[] data =
                Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        //METODO PARA CONFIGURAR LAS OPCIONES DE SEGURIDAD
        //DEL TOKEN
        //LOS METODOS DE CONFIGURACION SON ACTION
        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> jwtoptions =
                new Action<JwtBearerOptions>(options =>
                {
                    options.TokenValidationParameters =
                     new TokenValidationParameters()
                     {
                         ValidateActor = true, ValidateAudience = true, ValidateLifetime = true
                         , ValidateIssuerSigningKey = true
                         , ValidIssuer = this.Issuer
                         , ValidAudience = this.Audience
                         , IssuerSigningKey = this.GetKeyToken()
                     };
                });
            return jwtoptions;
        }

        //METODO PARA CONFIGURAR EL ESQUEMA DE AUTENTIFICACION
        public Action<AuthenticationOptions> GetAuthOptions()
        {
            Action<AuthenticationOptions> authoptions =
                new Action<AuthenticationOptions>(options => {
                    options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
                });
            return authoptions;
        }
    }
}
