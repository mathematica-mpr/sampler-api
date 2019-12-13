using System.Threading.Tasks;
using sampler_api.Models;

namespace sampler_api.Services
{
    public interface ISimulator
    {
        Task<Simulate> GetSimulate(Menu menu);
    }
}