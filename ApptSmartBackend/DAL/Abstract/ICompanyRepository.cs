using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.DAL.Abstract
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<bool> ExistsAsync(string companySlug);
        Task<Company?> GetBySlugAsync(string companySlug);
        Task<Company?> CreateCompanyAsync(Company company);
    }
}
