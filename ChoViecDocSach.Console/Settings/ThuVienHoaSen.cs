using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Onha.Kiet
{
    public class ThuVienHoaSen: GeneralSite
    {
        const string DOMAIN_HOST = @"http://thuvienhoasen.org";
        public ThuVienHoaSen (): base(DOMAIN_HOST)
        {
        }

        #region Override methods
        protected override HtmlNode GetContentDiv(string htmlContent)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlContent);

            var root = html.DocumentNode;
            var div = root.Descendants()
                              .Where(n => n.GetAttributeValue("class", "").Equals("pd_description nw_zoomcontent normal"))
                              .FirstOrDefault();
            var nodeToRemove = div.Descendants()
                              .Where(n => n.GetAttributeValue("class", "").Equals("nw_book_tree"))
                              .FirstOrDefault();

            var nodeToRemove2 = div.Descendants()
                              .Where(n => n.GetAttributeValue("class", "").Equals("nw_adspot nw_adspot_postcontent"))
                              .FirstOrDefault();

            var nodeToRemove3 = div.Descendants()
                              .Where(n => n.GetAttributeValue("class", "").Equals("clear"))
                              .FirstOrDefault();

            // this perfect to remove a node	
            // http://stackoverflow.com/questions/12092575/html-agility-pack-remove-element-but-not-innerhtml
            if (nodeToRemove!= null)
                nodeToRemove.ParentNode.RemoveChild(nodeToRemove, false);
            
            if (nodeToRemove2!= null)
                nodeToRemove2.ParentNode.RemoveChild(nodeToRemove2, false);
            
            if (nodeToRemove3!= null)
                nodeToRemove3.ParentNode.RemoveChild(nodeToRemove3, false);

            return div;
        }

        protected override IEnumerable<KeyValuePair<string, string>> GetLinks(string htmlContent)
        {
            var html = new HtmlDocument();
            html.LoadHtml(htmlContent);

            var root = html.DocumentNode;
            // the main div
            var div = root.Descendants()
                              .Where(n => n.GetAttributeValue("class", "").Equals("pd_description nw_zoomcontent normal"))
                              .FirstOrDefault();
            var chapter_div = div.Descendants()
                              .Where(n => n.GetAttributeValue("class", "").Equals("nw_book_tree"))
                              .FirstOrDefault();

            if (chapter_div!=null)
            {
                return chapter_div.Descendants("a")
                           .Where(n => n.Attributes["title"] != null)
                           .Select(item => new KeyValuePair<string, string>(
                               System.Net.WebUtility.HtmlDecode(item.InnerHtml), //key is name of each chapter
                               item.Attributes["href"].Value // value is the link
                               
                           ));
            }
            
            return null;
        }

        protected override Book GetBookInformation(HtmlNode contentNode)
        {
            var book = new Book();
            //style="font-size: medium;"
            
            //var span = contentNode.SelectSingleNode("//span[@style='font-size: medium;']");
            var texts = contentNode.Descendants("#text")
                                   .Where(n => n.HasChildNodes == false
                                     && n.InnerText.Contains((char)13) == false ) ; // <> '\r'
                                  
            if (texts!= null)
            {
                book.Title = texts.FirstOrDefault().InnerText;
                book.Copyright = "Thư Viện Hoa Sen";
                book.Creator = texts.ElementAt(1).InnerText;
                book.Publisher = "Thư Viện Hoa Sen";;
            }

            return book;
        }
        #endregion
        
    }
}