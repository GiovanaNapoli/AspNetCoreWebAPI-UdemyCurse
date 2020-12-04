using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        public ProfessorController(){}

        

        //retorna todos os professores
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Professores: Fulano, Ciclano, Beltrano");
        }

        //retornar professor especifico

    }
}