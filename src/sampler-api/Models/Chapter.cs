using System.Collections.Generic;

namespace sampler_api.Models
{

    public class ChapterElement
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }

    public class Chapter : ChapterElement
    {
        public List<ChapterElement> Descriptions { get; set; }
        public List<Menu> Menus { get; set; }
        public List<ChapterGraph> Graphs { get; set; }
    }

    public class Menu : Unique
    {
        public Menu(string guid)
        {
            GUID = guid;
        }
        public List<ChapterInput> Inputs { get; set; }
    }

    public class ChapterInput : ChapterElement
    {
        public ChapterInput()
        {

        }
        public List<ChapterInput> Inputs { get; set; }
        public float? Max { get; set; }
        public float? Min { get; set; }
        public float? Value { get; set; }
        public float? Step { get; set; }
    }


    public class Unique
    {
        public string GUID { get; set; }
    }


    public class ChapterGraph : ChapterElement
    {
        public ChapterGraph()
        {
            GraphItems = new List<GraphItem>();
        }
        public List<GraphItem> GraphItems { get; set; }
        public List<ChapterGraph> Graphs { get; set; }
    }

    public class GraphItem : Unique
    {
        public List<Coordinate> Coordinates { get; set; }
    }
}