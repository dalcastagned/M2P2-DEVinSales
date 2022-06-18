using DevInSales.Core.Data.Context;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Services
{
    public class StateService : IStateService
    {
        private readonly DataContext _context;

        public StateService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<State>> GetAll(string? name) =>
            await _context.States
                .Include(p => p.Cities)
                .Where(
                    p =>
                        !String.IsNullOrWhiteSpace(name)
                            ? p.Name.ToUpper().Contains(name.ToUpper())
                            : true
                )
                .ToListAsync();

        public async Task<State?> GetById(int stateId) =>
            await _context.States.Include(p => p.Cities).FirstOrDefaultAsync(p => p.Id == stateId);
    }
}
