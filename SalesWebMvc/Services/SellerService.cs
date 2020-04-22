using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(seller => seller.Department).FirstOrDefaultAsync(seller => seller.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var seller = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Cannot delete the seller beacuse he/she has salles!");
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool hasAny = await _context.Seller.AnyAsync(updatedSeller => updatedSeller.Id == seller.Id);
            if (hasAny)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new DbConcurrencyException(e.Message);
                }
            }
            else
            {
                throw new NotFoundException("Id not found!");
            }
        }
    }
}
