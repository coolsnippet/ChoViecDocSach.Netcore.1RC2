using System.Collections.Generic;
using HtmlAgilityPack;

namespace Onha.Kiet
{
    public class Book
    {
        public string Title { get; set; }
        public string Creator { get; set; }
        public string Copyright { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public IList<Chapter> Chapters { get; set; }
        public HtmlNode TableOfContent { get; set; }
        public Book()
        {
            Chapters = new List<Chapter>();
            
        }
    }
}
