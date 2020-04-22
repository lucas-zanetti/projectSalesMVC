using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var sales = from sale in _context.SalesRecord select sale;
            if (minDate.HasValue)
                sales = sales.Where(sale => sale.Date >= minDate.Value);
            if (maxDate.HasValue)
                sales = sales.Where(sale => sale.Date <= maxDate.Value);
            return await sales.Include(sale => sale.Seller).Include(sale => sale.Seller.Department).OrderByDescending(sale => sale.Date).ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var sales = from sale in _context.SalesRecord select sale;
            if (minDate.HasValue)
                sales = sales.Where(sale => sale.Date >= minDate.Value);
            if (maxDate.HasValue)
                sales = sales.Where(sale => sale.Date <= maxDate.Value);
            return await sales.Include(sale => sale.Seller).Include(sale => sale.Seller.Department).OrderByDescending(sale => sale.Date).GroupBy(sale=>sale.Seller.Department).ToListAsync();
        }
    }
}
