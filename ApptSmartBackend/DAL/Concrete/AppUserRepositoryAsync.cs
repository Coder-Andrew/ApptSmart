using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    public class AppUserRepositoryAsync : RepositoryAsync<UserInfo>, IAppUserRepositoryAsync
    {
        private readonly DbSet<UserInfo> _users;
        public AppUserRepositoryAsync(AppDbContext ctx) : base(ctx)
        {
            _users = ctx.UserInfos;
        }
    }
}
