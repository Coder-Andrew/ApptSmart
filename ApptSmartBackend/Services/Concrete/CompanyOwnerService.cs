using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Services.Concrete
{
    public class CompanyOwnerService : ICompanyOwnerService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserInfoRepositoryAsync _userInfoRepository;
        public CompanyOwnerService(ICompanyRepository companyRepository, IUserInfoRepositoryAsync userInfoRepository)
        {
            _companyRepository = companyRepository;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<bool> UserOwnsCompanyAsync(Guid userId, string companySlug)
        {
            var userInfo = await _userInfoRepository.FindByIdAsync(userId);
            if (userInfo == null) return false;

            var company = await _companyRepository.GetBySlugAsync(companySlug);
            if (company == null) return false;

            return userInfo.Id == company.OwnerId;
        }
    }
}
