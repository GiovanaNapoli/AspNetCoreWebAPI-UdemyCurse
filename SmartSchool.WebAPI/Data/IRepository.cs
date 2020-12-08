using SmartSchool.WebAPI.Models;

namespace SmartSchool.WebAPI.Data
{
    public interface IRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         bool SaveChanges();

         Aluno[] GetAllAlunos(bool includeProfessor = false);
         Aluno[] GetAllAlunosByDisciplinas(int disciplinaId, bool includeProfessor = false);
         Aluno GetAlunoById(int alunoId, bool includeProfessor = false);

         Professor[] GetAllProfessores( bool includeAlunos = false);
         Professor[] GetAllProfessoresByDisciplinas(int disciplinaId, bool includeAlunos =  false);
         Professor GetProfessorById(int professorId, bool includeProfessor = false);
    }
}