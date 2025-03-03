using AptSmartBackend.DAL.Abstract;
using AptSmartBackend.Models;
using AptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace AptSmartBackend.DAL.Concrete
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
