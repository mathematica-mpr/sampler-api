using System.Threading.Tasks;
using sampler_api.Models;

namespace sampler_api.Repositories
{
    public interface IChapterRepository
    {
        Task<Chapter> GetChapter();
        Task<Chapter> GetInitChapter(int id);
        Task<Chapter> GetUpdatedChapter(int id, SimulateParams sampleParams);
    }
}