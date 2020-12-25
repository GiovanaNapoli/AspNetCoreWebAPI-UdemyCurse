using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Data;
using SmartSchool.WebAPI.Dtos;
using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;

        public ProfessorController(IRepository repo, IMapper mapper){
            _repo = repo;
            _mapper = mapper;
        }
        //retorna todos os professores
        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllProfessores(true);

            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(result));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var prof = _repo.GetProfessorById(id, true);

            if (prof == null) return BadRequest("Aluno não encontrado");

            var profDto = _mapper.Map<ProfessorDto>(prof);

            return Ok(profDto);
        }

        [HttpGet("byaluno/{alunoId}")]
        public IActionResult GetByAlunoId(int alunoId)
        {
            var prof = _repo.GetProfessoresByAlunoId(alunoId, true);

            if (prof == null) return BadRequest("Professores não encontrado");

            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(prof));
        }

        [HttpPost]

        public IActionResult Post(ProfessorRegistrarDto model){

            var professor = _mapper.Map<Professor>(model);

            _repo.Add(professor);
            if( _repo.SaveChanges()){
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }
           return BadRequest("Professor não cadastrado");
            
        }

        [HttpPut("{id}")]

        public IActionResult Put(int id, ProfessorRegistrarDto model) {

            var professor = _repo.GetProfessorById(id, false);
            if(professor == null) return BadRequest("Professor não encontrado");

            _mapper.Map(model, professor);

            _repo.Update(professor);
            if( _repo.SaveChanges()){
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }
           return BadRequest("Professor não encontrado");
        }

        [HttpPatch("{id}")]

        public IActionResult Patch(int id, ProfessorRegistrarDto model) {

            var professor = _repo.GetProfessorById(id, false);
            if(professor == null) return BadRequest("Professor não encontrado");

            _mapper.Map(model, professor);

            _repo.Update(professor);
            if( _repo.SaveChanges()){
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
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