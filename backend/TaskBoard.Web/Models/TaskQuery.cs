using System.Collections.Generic;

namespace TaskBoard.Web.Models
{
    public class TaskQuery
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? SortBy { get; set; } = "desc"; // desc veya asc (CreatedAt)
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => PageSize > 0 ? (int)System.Math.Ceiling((double)TotalCount / PageSize) : 0;
    }
}