using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace Onha.Kiet
{
    public class MyNote : GeneralSite
    {
        const string DOMAIN_HOST = @"http://thuvienhoasen.org";
        public MyNote() : base(DOMAIN_HOST)
        {
        }

        #region Override methods
        protected override HtmlNode GetContentDiv(string htmlContent)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlContent);

            var root = html.DocumentNode;
            var div = root.SelectSingleNode("//body");        

            return div;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetLinks(string htmlContent)
        {         
            return null;
        }

        protected override Book GetBookInformation(HtmlNode contentNode)
        {
            var book = new Book();

            //var span = contentNode.SelectSingleNode("//span[@style='font-size: medium;']");
            var toc = contentNode.SelectSingleNode("//comment()[contains(., 'TOC')");
            var body = contentNode.SelectSingleNode("//body");
            var title = contentNode.SelectSingleNode("//h1)");
            book.Title = "Thích ca mâu ni";
            book.Creator = "Kiet Tran";
            book.Copyright = "Kiet Tran";
            book.Publisher = "Kiet Tran";

            if (title != null)
            {
                book.Title = System.Net.WebUtility.HtmlDecode(title.InnerText).Trim();                
            }

            if (toc != null)
            {
                book.TableOfContent = toc;
                book.Chapters.Add(new Chapter(){
                    Title = title.InnerText,
                    Content = body,
                    Number = 1
                });
            }

            return book;
        }

        protected override List<KeyValuePair<string, byte[]>> FixImages(HtmlNode div)
        {
            return null;
        }
        #endregion

    }
}