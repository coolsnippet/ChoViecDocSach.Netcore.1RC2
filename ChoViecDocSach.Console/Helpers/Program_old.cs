using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;
using System.Diagnostics;

namespace ConsoleApplication
{
    public class Program_temp
    {
        public static void Main1(string[] args)
        {
        //     var msg = "I love the World.";
        //     Console.WriteLine(msg);
        //     Console.WriteLine("Hello World!");
        // }

        // void Main()
        // {
            //instruction to create kindle file
               // http://www.aliciaramirez.com/2014/05/how-to-make-a-kindle-ebook-from-scratch/
            // C:\>C:\Kiet\Kinh\kindlegen.exe C:\Kiet\Kinh\Kinh_dai_bat_niet_ban\Kinh_dai_bat_niet_ban.opf
			System.Console.WriteLine(AppContext.BaseDirectory);
            // variables and in booklinks
            var output_downloadFile = @"/Users/kiettran/Downloads/Kinh_dai_bat_niet_ban.html";
            var title = Path.GetFileNameWithoutExtension(output_downloadFile).Replace("_", " ");
            var output_ncxFile = Path.Combine(Path.GetDirectoryName(output_downloadFile), Path.GetFileNameWithoutExtension(output_downloadFile) + ".ncx");
            var output_opfFile = Path.Combine(Path.GetDirectoryName(output_downloadFile), Path.GetFileNameWithoutExtension(output_downloadFile) + ".opf");

            // html document for ouputfile
            var output_html = new HtmlDocument();
            output_html.DocumentNode.AppendChild(HtmlNode.CreateNode("<html><head><meta charset=\"UTF-8\"></head><body></body></html>"));
            var output_head = output_html.DocumentNode.SelectSingleNode("/html/head");
            output_head.AppendChild(HtmlNode.CreateNode(string.Format("<title>{0}</title>", title)));
            output_head.AppendChild(HtmlNode.CreateNode(string.Format("<style>{0}</style>", ".center { text-align: center; } .pagebreak { page-break-before: always; }")));
            var output_body = output_html.DocumentNode.SelectSingleNode("/html/body");

            // 1. Get links, 2. get pairs of link and name of chapter
            var input_links = GetPairs(GetBook1Links());

            // 3. add table of content
            output_body.PrependChild(HtmlTableOfContent(input_links));
            output_body.AppendChild(HtmlNode.CreateNode("<div class=\"pagebreak\"></div>"));

            //4. delete existed file
            if (File.Exists(output_downloadFile)) File.Delete(output_downloadFile);
            if (File.Exists(output_ncxFile)) File.Delete(output_ncxFile);
            if (File.Exists(output_opfFile)) File.Delete(output_opfFile);

            NCXTableOfContent(title, input_links, output_downloadFile, output_ncxFile);
            OPFBookDetail(title, output_downloadFile, output_ncxFile, output_opfFile);

            int count = 1;
            foreach (var link in input_links)
            {
                var html = string.Empty;

            TRYAGAIN:
                try
                {
                    // 4. Download html content
                    html = Download(link.Value);
                }
                catch (WebException ex)
                {
                    System.Threading.Thread.Sleep(1000);
                    goto TRYAGAIN;
                }

                // 5. get the div, <-- new document, MODIFY
                var div = GetContentDiv(html);
                // add anchor link to it				
                output_body.AppendChild(HtmlNode.CreateNode(string.Format("<h2 id=\"ch{0}\" class=\"center\">{1}</h2>", count++, link.Key)));

                if (div != null)
                    output_body.AppendChild(div);

                // 6. write to console the current chapter
                Console.WriteLine(link.Key);

            }   // end loop all files	

            // 7. save to file
            //output_html.Save(output_downloadFile, Encoding.UTF8);	
			var output_stream = File.Create(output_downloadFile);
			output_html.Save(output_stream, Encoding.UTF8);
            //System.Console.WriteLine(output_html);
            // Create kindle file	
            //var kindleGenerator = " + "\"" + output_opfFile + "\"";
            //ProcessHelper.RunExternalExe(kindleGenerator);
			Process process = new Process();
			process.StartInfo.FileName = @"/Users/kiettran/Downloads/kindlegen";
			process.StartInfo.Arguments = output_opfFile;	
			process.StartInfo.CreateNoWindow = true;
			process.Start();
			process.WaitForExit();
			//System.Console.WriteLine(kindleGenerator);
        }

        static string Download(string url)
        {
			return new HttpClient().GetStringAsync(url).Result;

            // var request = WebRequest.Create(url);
            // request.Proxy = WebRequest.DefaultWebProxy;
            // request.Credentials = CredentialCache.DefaultCredentials;
            // request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            // Task<WebResponse> task = Task.Factory.FromAsync(
            //     request.BeginGetResponse,
            //     asyncResult => request.EndGetResponse(asyncResult),
            //     (object)null
            // );

            // using (Stream stream = task.Result.GetResponseStream())
            // using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            // {
            //     return reader.ReadToEnd();
            // }

            // using (var client = new WebRequest ())
            // {
            // 	client.Proxy = WebRequest.DefaultWebProxy;
            // 	client.Encoding = Encoding.UTF8;
            // 	client.Credentials = CredentialCache.DefaultCredentials;
            // 	client.Proxy.Credentials = CredentialCache.DefaultCredentials;

            // 	return client.DownloadString(new Uri(url));	
            // }

        }

        static string[] GetBook1Links()
        {
            string[] data =
        {
  "http://thuvienhoasen.org/p16a159/01-pham-tu-thu-nhat"                                                         ,"01. Phẩm Tự Thứ Nhất"
, "http://thuvienhoasen.org/p16a160/02-pham-thuan-da-thu-hai"                                                    ,"02. Phẩm Thuần Đà Thứ Hai"
, "http://thuvienhoasen.org/p16a161/03-pham-ai-than-thu-ba"                                                      ,"03. Phẩm Ai Thán Thứ Ba"
, "http://thuvienhoasen.org/p16a162/04-pham-truong-tho-thu-tu"                                                   ,"04. Phẩm Trường Thọ Thứ Tư"
, "http://thuvienhoasen.org/p16a163/05-pham-kim-cang-than-thu-nam"                                               ,"05. Phẩm Kim Cang Thân Thứ Năm"
, "http://thuvienhoasen.org/p16a164/06-pham-danh-tu-cong-duc-thu-sau"                                            ,"06. Phẩm Danh Tự Công Đức Thứ Sáu"
, "http://thuvienhoasen.org/p16a165/07-pham-tu-tuong-thu-bay"                                                    ,"07. Phẩm Tứ Tướng Thứ Bảy"
, "http://thuvienhoasen.org/p16a166/08-pham-tu-y-thu-tam"                                                        ,"08. Phẩm Tứ Y Thứ Tám"
, "http://thuvienhoasen.org/p16a167/09-pham-ta-chanh-thu-chin"                                                   ,"09. Phẩm Tà Chánh Thứ Chín"
, "http://thuvienhoasen.org/p16a168/10-pham-tu-de-thu-muoi"                                                      ,"10. Phẩm Tứ Đế Thứ Mười"
, "http://thuvienhoasen.org/p16a169/11-pham-tu-dao-thu-muoi-mot"                                                 ,"11. Phẩm Tứ Đảo Thứ Mười Một"
, "http://thuvienhoasen.org/p16a170/12-pham-nhu-lai-tanh-thu-muoi-hai"                                           ,"12. Phẩm Như Lai Tánh Thứ Mười Hai"
, "http://thuvienhoasen.org/p16a171/13-pham-van-tu-thu-muoi-ba"                                                  ,"13. Phẩm Văn Tự Thứ Mười Ba"
, "http://thuvienhoasen.org/p16a172/14-pham-dieu-du-thu-muoi-bon"                                                ,"14. Phẩm Điểu Dụ Thứ Mười Bốn"
, "http://thuvienhoasen.org/p16a173/15-pham-nguyet-du-thu-muoi-lam"                                              ,"15. Phẩm Nguyệt Dụ Thứ Mười Lăm"
, "http://thuvienhoasen.org/p16a174/16-pham-bo-tat-thu-muoi-sau"                                                 ,"16. Phẩm Bồ Tát Thứ Mười Sáu"
, "http://thuvienhoasen.org/p16a175/17-pham-dai-chung-so-van-thu-17"                                             ,"17. Phẩm Đại Chúng Sở Vấn Thứ 17"
, "http://thuvienhoasen.org/p16a176/18-pham-hien-binh-thu-muoi-tam"                                              ,"18. Phẩm Hiện Bịnh Thứ Mười Tám"
, "http://thuvienhoasen.org/p16a177/19-pham-thanh-hanh-thu-muoi-chin"                                            ,"19. Phẩm Thánh Hạnh Thứ Mười Chín"
, "http://thuvienhoasen.org/p16a178/20-pham-pham-hanh-thu-hai-muoi"                                              ,"20. Phẩm Phạm Hạnh Thứ Hai Mươi "
, "http://thuvienhoasen.org/p16a179/21-pham-anh-nhi-hanh-thu-hai-muoi-mot"                                       ,"21. Phẩm Anh Nhi Hạnh Thứ Hai Mươi Mốt"
, "http://thuvienhoasen.org/p16a180/22-pham-quang-minh-bien-chieu-cao-quy-duc-vuong-bo-tat-thu-hai-muoi-hai"     ,"22. Phẩm Quang Minh Biến Chiếu Cao Quý Đức Vương Bồ Tát Thứ Hai Mươi Hai"
, "http://thuvienhoasen.org/p16a181/23-pham-su-tu-hong-bo-tat-thu-hai-muoi-ba"                                   ,"23. Phẩm Sư Tử Hống Bồ Tát Thứ Hai Mươi Ba"
, "http://thuvienhoasen.org/p16a182/24-pham-ca-diep-bo-tat-thu-hai-muoi-bon"                                     ,"24. Phẩm Ca Diếp Bồ Tát  Thứ Hai Mươi Bốn"
, "http://thuvienhoasen.org/p16a183/25-pham-kieu-tran-nhu-thu-hai-muoi-lam"                                      ,"25. Phẩm Kiều Trần Như Thứ Hai Mươi Lăm"
, "http://thuvienhoasen.org/p16a184/26-pham-di-giao-thu-hai-muoi-sau"                                            ,"26. Phẩm Di Giáo Thứ Hai Mươi Sáu"
, "http://thuvienhoasen.org/p16a185/27-pham-ung-tan-huon-nguyen-thu-hai-muoi-bay"                                ,"27. Phẩm Ứng Tận Hườn Nguyên Thứ Hai Mươi Bảy"
, "http://thuvienhoasen.org/p16a186/28-pham-tra-ty-thu-hai-muoi-tam"                                             ,"28. Phẩm Trà Tỳ Thứ Hai Mươi Tám"
, "http://thuvienhoasen.org/p16a187/29-pham-cung-duong-xa-loi-thu-hai-muoi-chin"                                 ,"29. Phẩm Cúng Dường Xá Lợi Thứ Hai Mươi Chín"
};

            return data;

        }

        static IEnumerable<KeyValuePair<string, string>> GetPairs(string[] data)
        {
            for (int i = 0; i < data.Length / 2; i = i + 2)
            {
                yield return new KeyValuePair<string, string>(data[i + 1], data[i]);
            }
        }

        static HtmlNode GetContentDiv(string htmlContent)
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
            nodeToRemove.ParentNode.RemoveChild(nodeToRemove, false);
            //nodeToRemove2.ParentNode.RemoveChild(nodeToRemove2, false);
            nodeToRemove3.ParentNode.RemoveChild(nodeToRemove3, false);

            return div;
        }

        static HtmlNode HtmlTableOfContent(IEnumerable<KeyValuePair<string, string>> input_links)
        {
            var toc = HtmlNode.CreateNode("<div id=\"toc\" class=\"center\"></div>");
            var h2 = HtmlNode.CreateNode("<h2>Mục Lục</h2>");
            toc.PrependChild(h2);
            var ul = HtmlNode.CreateNode("<ul style=\"list-style:none\"></ul>");
            var count = 1;

            foreach (var link in input_links)
            {
                var il = HtmlNode.CreateNode(string.Format("<li><a href=\"#ch{0}\">{1}</a></li>", count++, link.Key));
                ul.AppendChild(il);
            }

            toc.AppendChild(ul);
            return toc;
        }

        static void NCXTableOfContent(string title, IEnumerable<KeyValuePair<string, string>> input_links, string htmlFilename, string ncxFilename)
        {
            var ncx = @"<?xml version=""1.0""?>
				     <!DOCTYPE ncx PUBLIC ""-//NISO//DTD ncx 2005-1//EN""
                     ""http://www.daisy.org/z3986/2005/ncx-2005-1.dtd"" >
                     <ncx xmlns = ""http://www.daisy.org/z3986/2005/ncx/"" version = ""2005-1"">
    				 <head>
    				 </head> 
				     <docTitle>
               	     <text>{0}</text>
        			 </docTitle>
					 {1}
					 </ncx>"
                             ;

            var count = 2;
            htmlFilename = Path.GetFileName(htmlFilename);

            var body = new XElement("navMap",
                                new XElement("navPoint", new XAttribute("id", "toc"), new XAttribute("playOrder", "1"),
                                        new XElement("navLabel",
                                            new XElement("text", "Mục Lục")
                                        ),
                                        new XElement("content", new XAttribute("src", htmlFilename + "#toc"))
                                ),

                                from p in input_links
                                select new XElement("navPoint", new XAttribute("id", "ch"), new XAttribute("playOrder", count),
                                        new XElement("navLabel",
                                            new XElement("text", p.Key)
                                        ),
                                        new XElement("content", new XAttribute("src", htmlFilename + "#ch" + (count++ - 1)))
                                )
                        );


            ncx = string.Format(ncx, title, body.ToString());

            File.WriteAllText(ncxFilename, ncx);
        }

        static void OPFBookDetail(string title, string htmlFilename, string ncxFilename, string opfBookFilename)
        {
            htmlFilename = Path.GetFileName(htmlFilename);
            ncxFilename = Path.GetFileName(ncxFilename);


            var opf = @"<?xml version=""1.0"" encoding=""iso-8859-1""?>
	<package unique-identifier=""uid"" xmlns:opf=""http://www.idpf.org/2007/opf"" xmlns:asd=""http://www.idpf.org/asdfaf"">
    <metadata>
        <dc-metadata  xmlns: dc = ""http://purl.org/metadata/dublin_core"" xmlns:oebpackage = ""http://openebook.org/namespaces/oeb-package/1.0/"" >
            <dc:Title> {0} </dc:Title>
            <dc:Language> en </dc:Language>
            <dc:Creator> Phat Thich Ca Mau Ni </dc:Creator>
            <dc:Copyrights> Phat Thich Ca Mau Ni </dc:Copyrights>
            <dc:Publisher> Online </dc:Publisher>            
        </dc-metadata>
    </metadata>
    <manifest>
        <item id=""content"" media-type=""application/xhtml+xml"" href=""{1}""></item>
        <item id=""ncx"" media-type=""application/xhtml+xml"" href=""{2}""/>
        <item id=""text"" media-type=""application/xhtml+xml"" href=""{1}""></item>            
    </manifest>
    <spine toc=""ncx"">
        <itemref idref=""content""/>
        <itemref idref=""text""/>       
    </spine>
    <guide>
        <reference type=""toc"" title=""Table of Contents"" href=""{1}""/>
        <reference type=""text"" title=""Book"" href=""{1}""/>
    </guide>
	</package>";

            opf = string.Format(opf, title, htmlFilename, ncxFilename);

            File.WriteAllText(opfBookFilename, opf);
        }
    }
}

