using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Integra.Shared.Base
{
    public class PaginatedList<T> : PaginatedListBase where T : class
    {
        public List<T> LosRegistros { get; set; }

        public PaginatedList()
        {
            PageIndex = 0;
            TotalPages = 0;
            LosRegistros = new List<T>();
            PáginaAnterior = 0;
            PáginaSiguiente = 0;
        }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            LosRegistros = new List<T>();
            LosRegistros.AddRange(items);
            PáginaAnterior = pageIndex-1;
            PáginaSiguiente = pageIndex+1;

        }


        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
			if (pageSize == 0)
				pageSize = 13;

            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
