using Marketplace.DAL.Context;
using Marketplace.DAL.IRepository;
using Marketplace.DAL.Models;
using Socialify.DAL.Repository;

namespace Marketplace.DAL.Repository
{
    public class CategoryRepository : Repo<Category>, ICategoryRepository
    {
        private readonly MarketplaceDbContext _dbContext;

        public CategoryRepository(MarketplaceDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
