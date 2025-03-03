using AptSmartBackend.DAL.Abstract;
using AptSmartBackend.Models;
using AptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace AptSmartBackend.DAL.Concrete
{
    public class AppUserRepository : Repository<UserInfo>, IAppUserRepository
    {
        private readonly DbSet<UserInfo> _users;
        public AppUserRepository(AppDbContext ctx) : base(ctx)
        {
            _users = ctx.UserInfos;
        }
    }
}
