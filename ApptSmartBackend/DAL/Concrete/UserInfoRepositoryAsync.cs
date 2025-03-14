using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    public class UserInfoRepositoryAsync : RepositoryAsync<UserInfo>, IUserInfoRepositoryAsync
    {
        private readonly DbSet<UserInfo> _usersInfos;
        public UserInfoRepositoryAsync(AppDbContext ctx) : base(ctx)
        {
            _usersInfos = ctx.UserInfos;
        }

        public Guid? GetUserId(string aspNetId)
        {
            UserInfo? userInfo = _usersInfos
                .FirstOrDefault(ui => ui.AspNetIdentityId == aspNetId);

            return userInfo?.Id;
        }
    }
}
