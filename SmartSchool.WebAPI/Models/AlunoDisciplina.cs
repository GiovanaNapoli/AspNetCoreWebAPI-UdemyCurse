namespace SmartSchool.WebAPI.Models
{
    public class AlunoDisciplina
    {
        public AlunoDisciplina(){}
        public AlunoDisciplina(int alunoId, int disciplinId )
        {
            this.AlunoId = alunoId;
            this.DisciplinId = disciplinId;

        }
        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; }
        public int DisciplinId { get; set; }
        public Disciplina Disciplina { get; set; }
    }
}