using Mix.Heart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Log.Lib.Models
{
    public class SearchLogResponseModel<T>
    {
        public List<SearchLogResult<T>> Results { get; set; }
        public PagingModel PagingData { get; set; }
        public SearchLogResponseModel()
        {
            Results = new List<SearchLogResult<T>>();
            PagingData = new PagingModel();
        }
    }
    public class SearchLogResult<T>
    {
        public DateTime SearchDate { get; set; }
        public PagingResponseModel<T> Data { get; set; }

    }
}