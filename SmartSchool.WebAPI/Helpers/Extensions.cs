using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SmartSchool.WebAPI.Helpers
{
    public static class Extensions
    {
        public static void AddPagination(this HttpResponse response, 
            int currentPage, int itemsPerPage, int totalItems, int totalPages){

            var camelCaseFormater = new JsonSerializerSettings();

            camelCaseFormater.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormater));
            response.Headers.Add("Access-Controle-Expose-Heade", "Pagination");
            
        }
    }
}