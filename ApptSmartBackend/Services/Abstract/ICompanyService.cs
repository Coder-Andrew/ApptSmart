using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Services.Abstract
{
    public interface ICompanyService
    {
        public Task<Company?> GetCompanyAsync(string companySlug);
        public Task<bool> CompanyExists(string companySlug);
        public Task<GenericResponse<Company>> CreateCompanyAsync(Company company);
    }
}
