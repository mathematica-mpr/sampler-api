using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sampler_api.Helpers;
using sampler_api.Models;

namespace sampler_api.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        public async Task<Menu> GetMenu()
        {
            using (StreamReader r = new StreamReader("configmenus.json"))
            {
                string json = await r.ReadToEndAsync();
                Menu menu = JsonConvert.DeserializeObject<Menu>(json);
                menu.GUID = Utils.GenerateGUID();
                return menu;
            }
        }
    }
}