using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WTR.Abstraction.Repository;
using WTR.Domain;

namespace WTR.DataAccess.Repository
{
    public interface ILocationRepository :IRepository<Location>
    {
        public Task<Location> GetByNameAsync(string name);
        public Task DeleteAllAsync();
    }
}
