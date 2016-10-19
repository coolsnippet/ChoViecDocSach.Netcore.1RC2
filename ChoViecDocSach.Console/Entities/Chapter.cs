using HtmlAgilityPack;

namespace Onha.Kiet
{
    public class Chapter
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public HtmlNode Content { get; set; }

    }
}