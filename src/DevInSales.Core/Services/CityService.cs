using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class CityService : ICityService
    {
        private readonly DataContext _context;

        public CityService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> GetAll(int stateId, string? name) =>
            await _context.Cities
                .Where(
                    p =>
                        p.StateId == stateId
                        && (
                            !String.IsNullOrEmpty(name)
                                ? p.Name.ToUpper().Contains(name.ToUpper())
                                : true
                        )
                )
                .ToListAsync();

        public async Task<City?> GetById(int cityId) =>
            await _context.Cities.Include(p => p.State).FirstOrDefaultAsync(p => p.Id == cityId);

        public async Task Add(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
        }
    }
}
