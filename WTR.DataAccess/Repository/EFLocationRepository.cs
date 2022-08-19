using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.DataAccess.Data;
using WTR.Domain;

namespace WTR.DataAccess.Repository
{
    public class EFLocationRepository : ILocationRepository
    {
        private readonly WeatherDBContext _context;

        public EFLocationRepository(WeatherDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Location entity)
        {
            await _context.Locations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task AddAsync(IEnumerable<Location> entity)
        {
            await _context.Locations.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Locations.FindAsync(id);
            _context.Locations.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _context.Locations.ToListAsync();

        }

        public async Task<Location> GetAsync(int id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task<Location> GetByNameAsync(string name)
        {

            return await _context.Locations.FirstOrDefaultAsync(x => x.Name.ToLowerInvariant() == name.ToLowerInvariant());

        }

        public async Task UpdateAsync(Location entity)
        {
            _context.Locations.Update(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAllAsync()
        {
            _context.Locations.RemoveRange(_context.Locations);
            await _context.SaveChangesAsync();

        }

        public async Task<IEnumerable<Location>> GetAsync(IEnumerable<int> id)
        {
            return await _context.Locations.Where(x => id.Contains(x.Id)).ToListAsync();
        }
    }
}
