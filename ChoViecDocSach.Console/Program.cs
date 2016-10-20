using Onha.Kiet;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {                
            var thuvienhoasen = new ThuVienHoaSen();         
            var bookHelper = new BookHelper(thuvienhoasen);
            
            //var firstUrlPath = @"/a15294/hieu-biet-ve-tanh-khong";            
            // var output_downloadFile = @"/Users/kiettran/Downloads/Kinh_dai_bat_niet_ban.html";
            // var firstUrlPath = @"/a17221/ban-do-tu-phat";
            // var firstUrlPath = @"/p27a10044/1/bai-van-khuyen-phat-tam-bo-de";
            var firstUrlPath = @"/p36a25595/ai-co-the-tho-gium-ai-";
            bookHelper.CreateKindleFiles(firstUrlPath);

        }
    }
}