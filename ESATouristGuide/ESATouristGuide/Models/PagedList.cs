using System;
using System.Collections.Generic;
using System.Text;

namespace ESATouristGuide.Models
{
    public class PagedList<T>
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
        public T[] Data { get; set; }

    }
}
