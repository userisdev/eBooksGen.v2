using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SkiaSharp;
using System.Text;

namespace eBooksGen.Libs
{
    /// <summary> PDFTools class. </summary>
    public static class PDFTools
    {
        /// <summary> Generates the specified directory information. </summary>
        /// <param name="target"> The target. </param>
        /// <param name="dst"> The destination. </param>
        /// <returns> pdf file. </returns>
        public static FileInfo Generate(BookInfo bookInfo, IEnumerable<string> items, string savePath)
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            using PdfDocument doc = new();
            doc.Info.Title = EncodingHack(bookInfo.Name);
            doc.Info.Author = EncodingHack(bookInfo.Author);
            doc.Info.Keywords = EncodingHack(bookInfo.Category);

            foreach (string path in items)
            {
                // XImageのサイズがおかしいためSkiaSharpで読み込みそのサイズを利用する
                using SKBitmap bmp = SKBitmap.Decode(path);
                using XImage ximg = XImage.FromFile(path);

                PdfPage page = doc.AddPage();
                page.Size = bmp.Height >= bmp.Width ? PageSize.B5 : PageSize.B4;
                page.Orientation = bmp.Height >= bmp.Width ? PageOrientation.Portrait : PageOrientation.Landscape;

                double ratioW = page.Width / bmp.Width;
                double ratioH = page.Height / bmp.Height;
                double ratio = Math.Min(ratioH, ratioW);

                double width = bmp.Width * ratio;
                double height = bmp.Height * ratio;
                double x = (page.Width - width) / 2;
                double y = (page.Height - height) / 2;
                XRect rect = new(x, y, width, height);

                using XGraphics g = XGraphics.FromPdfPage(page);
                g.SmoothingMode = XSmoothingMode.HighQuality;
                g.DrawImage(ximg, rect);
            }

            doc.Save(savePath);
            return new FileInfo(savePath);
        }

        /// <summary> Encodings the hack. </summary>
        /// <param name="text"> The text. </param>
        /// <returns> result. </returns>
        private static string EncodingHack(string text)
        {
            Encoding encoding = Encoding.BigEndianUnicode;
            byte[] bytes = encoding.GetBytes(text);
            StringBuilder sb = new();
            _ = sb.Append((char)254);
            _ = sb.Append((char)255);

            for (int i = 0; i < bytes.Length; ++i)
            {
                _ = sb.Append((char)bytes[i]);
            }

            return sb.ToString();
        }
    }
}
