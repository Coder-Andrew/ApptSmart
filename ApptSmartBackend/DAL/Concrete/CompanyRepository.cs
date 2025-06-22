using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly DbSet<Company> _companies;
        public CompanyRepository(AppDbContext ctx) : base(ctx)
        {
            _companies = ctx.Companies;
        }

        public async Task<Company?> CreateCompanyAsync(Company company)
        {
            await _companies.AddAsync(company);
            return company;
        }

        public async Task<bool> ExistsAsync(string companySlug)
        {
            return await _companies
                .AnyAsync(c => c.CompanySlug == companySlug);  
        }

        public async Task<Company?> GetBySlugAsync(string companySlug)
        {
            return await _companies
                .FirstOrDefaultAsync(c => c.CompanySlug == companySlug);
        }

        public async Task<bool> UserOwnsCompanyAsync(Guid userId)
        {
            return await _companies.AnyAsync(c => c.OwnerId == userId);
        }
    }
}
