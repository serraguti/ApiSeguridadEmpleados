using ApiSeguridadEmpleados.Helpers;
using ApiSeguridadEmpleados.Models;
using ApiSeguridadEmpleados.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiSeguridadEmpleados.Controllers
{
    //https://servicioempleadosapi/auth
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryEmpleados repo;
        private HelperOAuthToken helper;
        
        public AuthController(RepositoryEmpleados repo,
            IConfiguration configuration)
        {
            this.repo = repo;
            this.helper = new HelperOAuthToken(configuration);
        }

        //NECESITAMOS UN METODO PARA REALIZAR LA VALIDACION
        //LOS ENDPOINT DE OAUTH SON POST
        //RECIBIREMOS EL LOGINMODEL
        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(LoginModel model)
        {
            //VAMOS A VALIDAR DIRECTAMENTE CON EMPLEADOS
            Empleado empleado =
                this.repo.ExisteEmpleado(model.UserName, int.Parse(model.Password));
            if (empleado == null)
            {
                return Unauthorized();
            }
            else
            {
                //ALMACENAMOS EL DATO DEL EMPLEADO VALIDADO DENTRO 
                //DEL TOKEN
                string jsonempleado =
                    JsonConvert.SerializeObject(empleado);
                //LOS CLAIMS VAN EN ARRAY O COLECCION
                Claim[] claims = new[]
                {
                    new Claim("UserData", jsonempleado)
                };

                //UN TOKEN LLEVA UNAS CREDENCIALES
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken(),
                    SecurityAlgorithms.HmacSha256);
                //NECESITAMOS GENERAR UN TOKEN
                //EL TOKEN PUEDE LLEVAR INFORMACION DEL TIPO ISSUER, DURACION, 
                //CREDENCIALES DE USUARIO
                JwtSecurityToken token = 
                    new JwtSecurityToken(
                        claims: claims,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: credentials);
                //DEVOLVEMOS UNA RESPUESTA CORRECTA CON EL TOKEN
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }
    }
}
