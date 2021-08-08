using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tequila.Core
{
    public class QueryParams
    {
        [BindRequired]
        public int page { get; set; }
        
        [BindRequired]
        public int pageSize { get; set; }
    }
}