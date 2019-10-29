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
        public List<ChapterInput> Inputs { get; set; }
        public List<ChapterGraph> Graphs { get; set; }
    }

    public class ChapterInput : ChapterElement
    {
        public List<ChapterInput> Inputs { get; set; }
        public float? Max { get; set; }
        public float? Min { get; set; }
        public float? Init { get; set; }
        public float? Step { get; set; }
    }

    public class ChapterGraph : ChapterElement
    {
        public List<GraphItem> GraphItems { get; set; }
        public List<ChapterGraph> Graphs { get; set; }
    }

    public class GraphItem
    {
        public string GUID { get; set; }
        public List<Coordinate> Coordinates { get; set; }
    }
}