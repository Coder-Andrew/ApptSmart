using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.DAL.Abstract
{
    public interface IUserInfoRepositoryAsync : IRepositoryAsync<UserInfo>
    {
        Guid? GetUserId(string aspNetId);
    }
}
