namespace eBooksGen.Libs
{
    /// <summary> BookInfo record. </summary>
    /// <seealso cref="System.IEquatable&lt;eBooksGen.Libs.BookInfo&gt;" />
    public sealed record BookInfo(string Category, string Author, string Name);
}
