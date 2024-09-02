using TaskManager.Entities.Exceptions;

namespace TaskManager.Entities.Structs
{
    public struct Pagination<T>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public Pagination(int page, int pageSize) : this()
        {
            Page = page;
            PageSize = pageSize;
        }

        public IQueryable<T> Apply(IQueryable<T> entity)
        {
            if (Page < 1)
                throw new PaginationException("Page must be greater than 0");

            if (PageSize > 20)
                throw new PaginationException("PageSize value should not exceed 20");

            int skip = (Page - 1) * PageSize;
            return entity.Skip(skip).Take(PageSize);
        }
    }
}
