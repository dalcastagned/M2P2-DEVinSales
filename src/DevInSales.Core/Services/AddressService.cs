using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class AddressService : IAddressService
    {
        private readonly DataContext _context;

        public AddressService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetAll(
            int? stateId,
            int? cityId,
            string? street,
            string? cep
        )
        {
            var query = _context.Addresses
                .Include(a => a.City)
                .Include(a => a.City.State)
                .AsQueryable();
            if (cityId.HasValue)
                query = query.Where(x => x.CityId == cityId);
            if (stateId.HasValue)
                query = query.Where(x => x.City.StateId == stateId);
            if (!string.IsNullOrEmpty(street))
                query = query.Where(x => x.Street.ToUpper().Contains(street.ToUpper()));
            if (!string.IsNullOrEmpty(cep))
                query = query.Where(x => x.Cep == cep);
            return await query.ToListAsync();
        }

        public async Task Add(Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task<Address?> GetById(int addressId) =>
            await _context.Addresses.FirstOrDefaultAsync(p => p.Id == addressId);

        public async Task Delete(Address address)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
    }
}
