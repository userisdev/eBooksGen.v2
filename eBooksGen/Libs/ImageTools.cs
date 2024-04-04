using SkiaSharp;

namespace eBooksGen.Libs
{
    /// <summary> ImageTools class. </summary>
    public static class ImageTools
    {
        /// <summary> Saves the specified data. </summary>
        /// <param name="data"> The data. </param>
        /// <param name="path"> The path. </param>
        /// <param name="resizeFunc"> The resize function. </param>
        /// <returns> </returns>
        public static FileInfo Resize(string srcPath, string dstPath, Func<SKSize, SKSize> resizeFunc)
        {
            using SKBitmap bmp = SKBitmap.Decode(srcPath);
            SKSize newSize = resizeFunc.Invoke(new SKSize(bmp.Width, bmp.Height));
            using SKBitmap resized = bmp.Resize(new SKImageInfo((int)newSize.Width, (int)newSize.Height), SKFilterQuality.High);
            using SKSurface surface = SKSurface.Create(new SKImageInfo(resized.Width, resized.Height));
            using SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.White);
            canvas.DrawBitmap(resized, 0, 0);

            using SKImage img = surface.Snapshot();
            using SKData tmp = img.Encode(SKEncodedImageFormat.Jpeg, 75);

            if (File.Exists(dstPath))
            {
                File.Delete(dstPath);
            }

            using FileStream stream = new(dstPath, FileMode.CreateNew);
            tmp.SaveTo(stream);
            return new FileInfo(dstPath);
        }

        /// <summary> Resizes the specified source dir. </summary>
        /// <param name="srcDir"> The source dir. </param>
        /// <param name="dstDir"> The DST dir. </param>
        /// <param name="resizeFunc"> The resize function. </param>
        /// <returns> </returns>
        public static IEnumerable<FileInfo> Resize(DirectoryInfo srcDir, DirectoryInfo dstDir, Func<SKSize, SKSize> resizeFunc)
        {
            FileInfo[] files = srcDir.GetFiles("*", SearchOption.TopDirectoryOnly);
            List<FileInfo> results = [];
            foreach (FileInfo file in files)
            {
                string dstPath = Path.Combine(dstDir.FullName, Path.ChangeExtension(file.Name, ".jpg"));
                FileInfo result = Resize(file.FullName, dstPath, resizeFunc);
                results.Add(result);
            }

            return results;
        }
    }
}
