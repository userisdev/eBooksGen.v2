using System.Text.RegularExpressions;

namespace eBooksGen.Libs
{
    /// <summary> BookTools class. </summary>
    public static partial class BookTools
    {
        /// <summary> Parses the specified dir name. </summary>
        /// <param name="dirName"> Name of the dir. </param>
        /// <returns> </returns>
        public static BookInfo Parse(string dirName)
        {
            Match match = BookInfoRegex().Match(dirName);
            string category = match.Groups[1].Value;
            string auther = match.Groups[2].Value;
            string name = match.Groups[3].Value;
            return new BookInfo(category, auther, name);
        }

        /// <summary> Books the information regex. </summary>
        /// <returns> </returns>
        [GeneratedRegex(@"^\[(.*?)\]\[(.*?)\](.*?)$")]
        private static partial Regex BookInfoRegex();
    }
}
