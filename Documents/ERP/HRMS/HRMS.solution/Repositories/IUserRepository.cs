using Microsoft.AspNetCore.Identity;

namespace HRMS.solution.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}

