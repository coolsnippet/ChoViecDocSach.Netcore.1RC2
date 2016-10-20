using System.Collections.Generic;
using HtmlAgilityPack;

namespace Onha.Kiet
{
    public class Chapter
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public HtmlNode Content { get; set; }
        public List<KeyValuePair<string, byte[]>> Images { get; set;}
        public Chapter ()
        {
           Images = new List<KeyValuePair<string, byte[]>>();
        }

    }
}