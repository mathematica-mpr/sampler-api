using System.Threading.Tasks;
using sampler_api.Models;

namespace sampler_api.Repositories
{
    public interface IMenuRepository
    {
        Task<Menu> GetMenu();
    }
}