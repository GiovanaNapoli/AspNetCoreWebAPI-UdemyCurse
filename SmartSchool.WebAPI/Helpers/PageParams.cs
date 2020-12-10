namespace SmartSchool.WebAPI.Helpers
{
    public class PageParams
    {
        public const int MaxPageSize = 50;
        public int PageNumber { get; set; }
        private int pageSize = 10;
        public int PageSize { 
            get{return pageSize;} 
            set{pageSize = (value > MaxPageSize) ? MaxPageSize : value;} 
        }

        public int? Matricula { get; set; } = null;
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; }
        public int? Ativo { get; set; } = null;
    }
}