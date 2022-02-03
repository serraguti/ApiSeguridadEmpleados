using ApiSeguridadEmpleados.Models;
using ApiSeguridadEmpleados.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridadEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public List<Empleado> GetEmpleados()
        {
            return this.repo.GetEmpleados();
        }

        [HttpGet("{id}")]
        public Empleado GetEmpleado(int id)
        {
            return this.repo.FindEmpleado(id);
        }
    }
}
