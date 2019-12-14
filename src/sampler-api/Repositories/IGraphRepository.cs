using System.Collections.Generic;
using System.Threading.Tasks;
using sampler_api.Models;

namespace sampler_api.Repositories
{
    public interface IGraphRepository
    {
        Task<List<Graph>> GetGraphs(SimulateParams simulateParams);
    }
}