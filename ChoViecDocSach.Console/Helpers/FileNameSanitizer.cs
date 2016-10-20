using System.Text.RegularExpressions;

namespace Onha.Kiet
{
    public class FileNameSanitizer
    {
        public static string GiveGoodName(string originalFilename)
        {
            var invalids = System.IO.Path.GetInvalidFileNameChars();
            return System.String.Join("_", originalFilename.Split(invalids, System.StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.').TrimEnd(',');
        }

        public static bool IsBadName(string originalFilename)
        {
            Regex containsABadCharacter = new Regex("["  + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
            if (containsABadCharacter.IsMatch(originalFilename)) { return true; };

            // other checks for UNC, drive-path format, etc

            return false;
        }
    }
}