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
            if (nodeToRemove != null)
                nodeToRemove.ParentNode.RemoveChild(nodeToRemove, false);

            if (nodeToRemove2 != null)
                nodeToRemove2.ParentNode.RemoveChild(nodeToRemove2, false);

            if (nodeToRemove3 != null)
                nodeToRemove3.ParentNode.RemoveChild(nodeToRemove3, false);

            var tableNodes = div.SelectNodes("//table");
            if (tableNodes != null)
            {
                foreach (var node in tableNodes)
                {
                    node.ParentNode.RemoveChild(node, true);
                }
            }


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

            if (chapter_div != null)
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

                                     && System.Net.WebUtility.HtmlDecode(n.InnerText).Contains((char)13) == false // <> '\r'
                                     & !System.Net.WebUtility.HtmlDecode(n.InnerText).Contains("Tủ Sách Đạo Phật Ngày Nay") // ignore this title
                                     & !string.IsNullOrEmpty(System.Net.WebUtility.HtmlDecode(n.InnerText).Trim()) // no empty line
                                     );

            book.Title = "Thích ca mâu ni";
            book.Creator = "Thích ca mâu ni";
            book.Copyright = "Thư Viện Hoa Sen";
            book.Publisher = "Thư Viện Hoa Sen";

            if (texts != null)
            {
                book.Title = System.Net.WebUtility.HtmlDecode(texts.FirstOrDefault().InnerText).Trim();
                book.Creator = System.Net.WebUtility.HtmlDecode(texts.ElementAt(1).InnerText).Trim();

                if (book.Title.EndsWith(",")
                || book.Title.EndsWith(":")
                )
                {
                    book.Title = book.Title + book.Creator;
                    book.Creator = System.Net.WebUtility.HtmlDecode(texts.ElementAt(2).InnerText).Trim();
                }
            }

            return book;
        }

        protected override List<KeyValuePair<string, byte[]>> FixImages(HtmlNode div)
        {
            var imgNodes = div.Descendants("img");// .SelectNodes("//img");
            var images = new List<KeyValuePair<string, byte[]>>();

            foreach (var node in imgNodes)
            {
                var imagePath = node.GetAttributeValue("data-original", "");
                if (string.IsNullOrEmpty(imagePath))
                    imagePath = node.GetAttributeValue("src", "");

                var imageFile = System.IO.Path.GetFileName(imagePath);

                if (!FileNameSanitizer.IsBadName(imageFile))
                {
                    var imageBytesTask = webber.DownloadFile(imagePath);
                    byte[] imageBytes = null;

                    try
                    {
                        imageBytes = imageBytesTask.Result;

                        if (imageBytesTask.Status != System.Threading.Tasks.TaskStatus.Faulted)
                        {
                            images.Add(new KeyValuePair<string, byte[]>(imageFile, imageBytes));
                        }
                        node.SetAttributeValue("src", imageFile); // modify the name in source
                    }
                    catch (System.AggregateException ex)
                    {
                        // node.RemoveChild(node);
                    }
                    finally
                    {

                    }


                }
            }

            return images;
        }
        #endregion

    }
}