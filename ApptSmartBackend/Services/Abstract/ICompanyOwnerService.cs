namespace ApptSmartBackend.Services.Abstract
{
    public interface ICompanyOwnerService
    {
        public Task<bool> UserOwnsCompanyAsync(Guid userId, string companySlug);
    }
}
