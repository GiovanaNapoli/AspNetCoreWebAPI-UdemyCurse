using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repo;

        public ProfessorController(IRepository repo){
            _repo = repo;
        }
        //retorna todos os professores
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllProfessores(true);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var prof = _repo.GetProfessorById(id, false);

            if (prof == null) return BadRequest("Aluno não encontrado");

            return Ok(prof);
        }

        [HttpPost]

        public IActionResult Post(Professor professor){
            _repo.Add(professor);
            if( _repo.SaveChanges()){
                return Ok(professor);
            }
           return BadRequest("Professor não cadastrado");
            
        }

        [HttpPut("{id}")]

        public IActionResult Put(int id, Professor professor) {

            var prof = _repo.GetProfessorById(id, false);
            if(prof == null) return BadRequest("Professor não encontrado");

            _repo.Update(professor);
            if( _repo.SaveChanges()){
                return Ok(professor);
            }
           return BadRequest("Professor não encontrado");
        }

        [HttpPatch("{id}")]

        public IActionResult Patch(int id, Professor professor) {

            var prof = _repo.GetProfessorById(id, false);
            if(prof == null) return BadRequest("Professor não encontrado");

            _repo.Update(professor);
            if( _repo.SaveChanges()){
                return Ok(professor);
            }
           return BadRequest("Professor não encontrado");
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id) {

            var prof = _repo.GetProfessorById(id, false);
            if(prof == null) return BadRequest("Professor não encontrado");

            _repo.Delete(prof);
            if( _repo.SaveChanges()){
                return Ok("Professor deletado");
            }
           return BadRequest("Professor não encontrado");
        }

        //retornar professor especifico

    }
}