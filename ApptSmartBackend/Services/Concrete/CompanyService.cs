using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Services.Concrete
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<Company?> GetCompanyAsync(string companySlug)
        {
            return await _companyRepository.GetBySlugAsync(companySlug);
        }

        public Task CreateCompanyAsync(Company company)
        {
            throw new NotImplementedException();
        }

    }
}
