using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Pagination
{
    public readonly record struct PagedRequest
    {
        public PagedRequest(int page = 1, int pageSize = 20)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 1 : Math.Min(pageSize, 100);
        }

        public int Page { get; }
        public int PageSize { get; }

        public int Skip => (Page - 1) * PageSize;
        public int Take => PageSize;
    }
}
