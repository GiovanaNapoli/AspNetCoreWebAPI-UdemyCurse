using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using SmartSchool.WebAPI.Models;
using SmartSchool.WebAPI.Dtos;
using AutoMapper;
using System.Threading.Tasks;
using SmartSchool.WebAPI.Helpers;

namespace SmartSchool.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AlunoController : ControllerBase
    {
        public readonly IRepository _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public AlunoController( IRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// Método responsável por fazer a listagem de alunos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]PageParams pageParams)
        {
            var result = await _repo.GetAllAlunosAsync(pageParams, true);

            var alunosResult = _mapper.Map<IEnumerable<AlunoDto>>(result);
            
            Response.AddPagination(result.CurrentPage, result.PageSize, result.TotalCount, result.TotalPages);

            return Ok(alunosResult);
        }

        /// <summary>
        /// Método responsável por listar os alunos de acordo com o ID passado por rota
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);

            if (aluno == null) return BadRequest("Aluno não encontrado");

            var alunoDto = _mapper.Map<AlunoRegistrarDto>(aluno);

            return Ok(alunoDto);
        }
        [HttpGet("ByDisciplina/{id}")]
        public async Task<IActionResult> GetDisciplinaById(int id)
        {
            var result = await _repo.GetAllAlunosByDisciplinasAsync(id, false);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(AlunoRegistrarDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);
            if(_repo.SaveChanges()){
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            return BadRequest("Aluno não cadastrado");

            // _repo.Add(aluno);
            // var result = _repo.SaveChanges() ? "Aluno cadastrado" : BadRequest("Erro ao cadastrar");
            // return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {
            var aluno = _repo.GetAlunoById(id, false);
            if (aluno == null) return BadRequest("Aluno não encontrado");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if(_repo.SaveChanges()){
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            return BadRequest("Aluno não atualizado");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoPatchDto model)
        {

            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("Aluno não encontrado");

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if(_repo.SaveChanges()){
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoPatchDto>(aluno));
            }
            return BadRequest("Aluno não atualizado");
        }

        [HttpPatch("{id}/trocarEstado")]
        public IActionResult trocarEstado(int id, TrocarEstado trocarEstado)
        {

            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("Aluno não encontrado");

            aluno.Ativo = trocarEstado.Estado;

            _repo.Update(aluno);
            if(_repo.SaveChanges()){
                var msn = aluno.Ativo ? "ativado" : "desativado";
                return Ok(new {message = $"Aluno {msn} com sucesso!"});
            }
            return BadRequest("Aluno não atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAlunoById(id, false);
            if (aluno == null) return BadRequest("Aluno não encontrado");

            _repo.Delete(aluno);
            if(_repo.SaveChanges()){
                return Ok("Aluno Deletado");
            }
            return BadRequest("Aluno não Deletado");
        }
    }
}