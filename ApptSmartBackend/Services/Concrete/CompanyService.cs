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

        public async Task<bool> CompanyExists(string companySlug)
        {
            return await _companyRepository.ExistsAsync(companySlug);
        }

        public async Task<GenericResponse<Company>> CreateCompanyAsync(Company company)
        {
            if (await _companyRepository.ExistsAsync(company.CompanySlug))
            {
                return new GenericResponse<Company>
                {
                    Data = null,
                    Success = false,
                    Message = "Duplicate company slug",
                    StatusCode = GenericStatusCode.DuplicateCompanySlug
                };
            }

            _companyRepository.Add(company);

            return new GenericResponse<Company>
            {
                Data = company,
                Success = true,
                StatusCode = GenericStatusCode.CompanyCreated,
                Message = $"Company '{company.CompanySlug}' with id {company.Id} created successfully"
            };
        }
    }
}
