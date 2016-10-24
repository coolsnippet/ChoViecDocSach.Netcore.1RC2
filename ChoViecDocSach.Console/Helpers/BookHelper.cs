using System.IO;
using System.Xml.Linq;
using System.Linq;
using HtmlAgilityPack;
using System.Text;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace Onha.Kiet
{
    // Links 
    // get image
    // https://social.msdn.microsoft.com/Forums/vstudio/en-US/a87e6ec2-248b-4c67-9c8b-e10aa637b275/how-can-i-get-images-names-to-a-liststring-from-a-url-of-a-website-using-htmlagilitypack-?forum=csharpgeneral
    public class BookHelper
    {
        private Book book;
        string htmlFilename;
        string ncxFilename;
        string opfBookFilename;
        GeneralSite website;
        public string KindlegenPath { get; set; }
        public string DownloadFolder { get; set; }
        public BookHelper(GeneralSite website)
        {
            this.website = website;
        }

        // include 3 files: html (content), opf (main file) and ncx (content file)
        // http://www.aliciaramirez.com/2014/05/how-to-make-a-kindle-ebook-from-scratch/
        public string CreateKindleFiles(string firstpageUrl, string alreadyHtmlFile = null)
        {
            // 1. get the book base on website structure
            book = website.GetOneWholeHtml(firstpageUrl);
            // 2. assign 3 file names
            var title = VietnameseAccentsRemover.RemoveSign4VietnameseString(book.Title);
            title = title.Replace(" ", "_");
            title = FileNameSanitizer.GiveGoodName(title).Trim();

            if (string.IsNullOrEmpty(title))
                return string.Empty;

            var downloadFolder = "";
            var trashFolder = "";

            // get os type and set download path and kindle file
            if (Environment.GetEnvironmentVariable("_system_name") == "OSX")
            {
                trashFolder = Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".Trash");
                downloadFolder = Path.Combine(Environment.GetEnvironmentVariable("HOME"), "Downloads");
                KindlegenPath = "/Users/kiettran/Downloads/kindlegen";
            }
            else // windows
            {
                trashFolder = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
                downloadFolder = Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
                KindlegenPath = @"C:\Kiet\Kinh\kindlegen.exe";
            }

            // in case we use Website project to reference to it
            if (!string.IsNullOrEmpty(DownloadFolder))
            {
                downloadFolder = DownloadFolder;
            }

            htmlFilename = Path.Combine(trashFolder, title) + ".html";

            ncxFilename = Path.Combine(Path.GetDirectoryName(htmlFilename), Path.GetFileNameWithoutExtension(htmlFilename) + ".ncx");
            opfBookFilename = Path.Combine(Path.GetDirectoryName(htmlFilename), Path.GetFileNameWithoutExtension(htmlFilename) + ".opf");
            // 3. create 3 files
            CreateHtmlFile(alreadyHtmlFile);
            CreateNCXTableOfContent();
            CreateOPFBookDetail();

            // 4. Create mobile kindle file
            if (!string.IsNullOrWhiteSpace(KindlegenPath))
            {
                CreateKindleFile();
                // we don't use these files, delete it for now
                htmlFilename = Path.Combine(trashFolder, htmlFilename);
                ncxFilename = Path.Combine(trashFolder, ncxFilename);
                opfBookFilename = Path.Combine(trashFolder, opfBookFilename);

                if (File.Exists(htmlFilename)) File.Delete(htmlFilename);
                if (File.Exists(ncxFilename)) File.Delete(ncxFilename);
                if (File.Exists(opfBookFilename)) File.Delete(opfBookFilename);

                foreach (var image in book.Chapters.SelectMany(c => c.Images))
                {
                    var imageFilename = Path.Combine(trashFolder, image.Key);
                    if (File.Exists(imageFilename)) File.Delete(imageFilename);
                }

                var trashMobiFileName = Path.Combine(trashFolder, title) + ".mobi";
                var downloadMobiFileName = Path.Combine(downloadFolder, title) + ".mobi";

                if (trashMobiFileName != downloadMobiFileName) // on Windows, we don't need to copy because I didn't check for trash folder
                {
                    if (File.Exists(downloadMobiFileName)) File.Delete(downloadMobiFileName);
                    File.Move(trashMobiFileName, downloadMobiFileName);
                }

            }

            return Path.Combine(downloadFolder, title) + ".mobi";

        }

        private void CreateOPFBookDetail()
        {
            htmlFilename = Path.GetFileName(htmlFilename);
            ncxFilename = Path.GetFileName(ncxFilename);


            var opf = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                        <package unique-identifier=""uid"" xmlns:opf=""http://www.idpf.org/2007/opf"" xmlns:asd=""http://www.idpf.org/asdfaf"">
                        <metadata>
                            <dc-metadata  xmlns: dc = ""http://purl.org/metadata/dublin_core"" xmlns:oebpackage = ""http://openebook.org/namespaces/oeb-package/1.0/"" >
                                <dc:Title> {0} </dc:Title>
                                <dc:Language> en </dc:Language>
                                <dc:Creator> {3} </dc:Creator>
                                <dc:Copyrights> {4} </dc:Copyrights>
                                <dc:Publisher> {5} </dc:Publisher>            
                            </dc-metadata>
                        </metadata>
                        <manifest> 
                            <item id=""ncx"" media-type=""application/xhtml+xml"" href=""{2}""/>
                            <item id=""text"" media-type=""application/xhtml+xml"" href=""{1}""></item>            
                        </manifest>
                        <spine toc=""ncx"">                       
                            <itemref idref=""text""/>       
                        </spine>
                        <guide>
                            <reference type=""toc"" title=""Table of Contents"" href=""{1}""/>
                            <reference type=""text"" title=""Book"" href=""{1}""/>
                        </guide>
                        </package>";

            opf = string.Format(opf
            , book.Title
            , htmlFilename
            , ncxFilename
            , book.Creator
            , book.Copyright
            , book.Publisher);

            File.WriteAllText(opfBookFilename, opf);
        }

        private void CreateNCXTableOfContent()
        {
            var ncx = @"<?xml version=""1.0"" encoding=""UTF-8""?>
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

            //var count = 2;
            htmlFilename = Path.GetFileName(htmlFilename);

            var body = new XElement("navMap",
                                new XElement("navPoint", new XAttribute("id", "toc"), new XAttribute("playOrder", "1"),
                                        new XElement("navLabel",
                                            new XElement("text", "Mục Lục")
                                        ),
                                        new XElement("content", new XAttribute("src", htmlFilename + "#toc"))
                                ),

                                from chapter in book.Chapters
                                select new XElement("navPoint", new XAttribute("id", "ch" + chapter.Number), new XAttribute("playOrder", (chapter.Number + 1)),
                                        new XElement("navLabel",
                                            new XElement("text", chapter.Title)
                                        ),
                                        new XElement("content", new XAttribute("src", htmlFilename + "#ch" + chapter.Number))
                                )
                        );


            ncx = string.Format(ncx, book.Title, body.ToString());

            File.WriteAllText(ncxFilename, ncx);
        }

        private void CreateHtmlFile(string alreadyHtmlFile = null)
        {
            // in md note, we already have html file 
            if (alreadyHtmlFile!= null)
            {
                if (File.Exists(htmlFilename)) File.Delete(htmlFilename);
                File.Copy(alreadyHtmlFile, htmlFilename);
            }            

            // 1. html document for ouputfile
            var output_html = new HtmlDocument();
            output_html.DocumentNode.AppendChild(HtmlNode.CreateNode("<html><head><meta charset=\"UTF-8\"></head><body></body></html>"));
            var output_head = output_html.DocumentNode.SelectSingleNode("/html/head");
            output_head.AppendChild(HtmlNode.CreateNode(string.Format("<title>{0}</title>", book.Title)));
            output_head.AppendChild(HtmlNode.CreateNode(string.Format("<style>{0}</style>", ".center { text-align: center; } .pagebreak { page-break-before: always; }")));
            var output_body = output_html.DocumentNode.SelectSingleNode("/html/body");

            // 2. add table of content
            if (book.TableOfContent != null)
            {
                output_body.PrependChild(book.TableOfContent);
                output_body.AppendChild(HtmlNode.CreateNode("<div class=\"pagebreak\"></div>"));
            }

            foreach (var chapter in book.Chapters)
            {
                // 3. add anchor link to it			
                output_body.AppendChild(HtmlNode.CreateNode("<div class=\"pagebreak\"></div>")); // add page break for new link
                output_body.AppendChild(HtmlNode.CreateNode(string.Format("<h2 id=\"ch{0}\" class=\"center\">{1}</h2>", chapter.Number, chapter.Title)));
                output_body.AppendChild(chapter.Content); ;

                // 4. and download images
                var imagePath = Path.GetDirectoryName(htmlFilename);
                SaveImagesToFiles(chapter.Images, imagePath);
            }
            // 5. remove style attributes (font-size)
            // RemoveHardStyleSize(output_body);

            // 6.output_html.Save(output_downloadFile, Encoding.UTF8);	
            if (File.Exists(htmlFilename)) File.Delete(htmlFilename);

            using (var output_stream = File.Create(htmlFilename))
            {
                output_html.Save(output_stream, Encoding.UTF8);
            }

        }

        private void RemoveHardStyleSize(HtmlNode node)
        {
            try
            {
                var elementsWithStyleAttribute = node.SelectNodes("//@style");

                if (elementsWithStyleAttribute != null)
                {
                    foreach (var element in elementsWithStyleAttribute)
                    {
                        if (element.Attributes["style"].Value.Contains("font-size"))
                            element.Attributes["style"].Remove();
                    }
                }
            }
            catch (System.Exception)
            {

            }

        }

        private void SaveImagesToFiles(List<KeyValuePair<string, byte[]>> images, string path)
        {
            if (images == null)
                return;

            foreach (var item in images)
            {
                var filename = Path.Combine(path, item.Key);
                using (var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    fileStream.Write(item.Value, 0, item.Value.Length);
                }
            }

        }

        private void CreateKindleFile()
        {
            Process process = new Process();
            process.StartInfo.FileName = KindlegenPath; //@"/Users/kiettran/Downloads/kindlegen";
            process.StartInfo.Arguments = opfBookFilename;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
            process.WaitForExit();
        }
    }
}