using System.Collections;
using System.Collections.Generic;

namespace Tequila.Core
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}