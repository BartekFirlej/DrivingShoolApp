using DrivingSchoolApp.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DrivingSchoolApp.DTOs
{
    public class PagedList<T>
    {
        public List<T> PagedItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public PagedList() { }
        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
                throw new ValueMustBeGreaterThanZeroException("page number");
            if (pageSize <= 0)
                throw new ValueMustBeGreaterThanZeroException("page size");
            var page = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize + 1).ToListAsync();
            return new PagedList<T>
            {
                PagedItems = new List<T>(page.Take(pageSize).ToList()),
                PageIndex = pageIndex,
                PageSize = pageSize,
                HasNextPage = page.Skip(pageSize).ToList().Any()
            };
        }
    }
}
