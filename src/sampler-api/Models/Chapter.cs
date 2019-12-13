using System.Collections.Generic;

namespace sampler_api.Models
{

    public class BaseElement
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }

    public class Chapter : BaseElement
    {
        public List<BaseElement> Descriptions { get; set; }
        public List<Menu> Menus { get; set; }
        public List<Graph> Graphs { get; set; }
    }

}