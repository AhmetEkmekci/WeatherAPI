using WTR.Business.DTO;

namespace WTR.Business.Service
{
    public interface ILocationService
    {
        Task AddLocationAsync(LocationDto addLocationRequest);
        Task<LocationDto> GetByIdAsync(int id);
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync();

        /// <summary>
        /// For test purpose.
        /// </summary>
        Task<int> InitLocationsAsync(string LocationNameQuery);
        /// <summary>
        /// For test purpose.
        /// </summary>
        Task DeleteAll();
    }
}