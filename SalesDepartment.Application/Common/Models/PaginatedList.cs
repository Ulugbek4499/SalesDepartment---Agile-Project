using Microsoft.EntityFrameworkCore;

namespace SalesDepartment.Application.Common.Models
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public List<T> Items { get; private set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (decimal)pageSize);
            TotalCount = count;
            Items = items;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T>? source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
        public static async Task<PaginatedList<T>> CreateAsync(List<T>? source, int pageNumber, int pageSize)
        {
            var count = source.Count;
            List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return await Task.FromResult(new PaginatedList<T>(items, count, pageNumber, pageSize));
        }
    }
}
