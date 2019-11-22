using System.Collections.Generic;
using System.Threading.Tasks;
using sampler_api.Models;

namespace sampler_api.Repositories
{
    public interface IChapterRepository
    {
        Task<Chapter> GetChapter();
        Task<Chapter> GetInitChapter();
        Task<List<ChapterGraph>> UpdateGraphs(SimulateParams simulateParams, List<ChapterGraph> chapterGraphs);
    }
}