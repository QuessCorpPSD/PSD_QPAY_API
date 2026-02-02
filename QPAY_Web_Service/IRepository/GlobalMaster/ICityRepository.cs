using QPay.UI.Models.GlobalMaster;

namespace QPay.BAL.IRepository.GlobalMaster
{
    public interface ICityRepository
    {
        Task<List<City>> GetAllCity(string? cityName, int? stateId, int? cityId);
        Task<string> Create(string xml, string mode, int createdBy);

        Task<List<Circle>> GetAllCircle(int stateId);
    }
}
