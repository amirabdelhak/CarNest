using System;
using System.Collections.Generic;

namespace Presentation.DTOs.Responses
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize > 0 ? pageSize : 1;         // avoid division by zero
            TotalRecords = Math.Max(0, totalRecords);
            TotalPages = PageSize > 0 ? (int)Math.Ceiling(TotalRecords / (double)PageSize) : 0;
            Data = data;
        }
    }
}
