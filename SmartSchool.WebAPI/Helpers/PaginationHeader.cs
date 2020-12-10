namespace SmartSchool.WebAPI.Helpers
{
    public class PaginationHeader
    {
        public int CurrentPage { get; set; }
        public int ItemsPerSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public PaginationHeader(int currentPage, int itemsPerSize, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerSize = itemsPerSize;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;

        }
    }
}