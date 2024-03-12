using SkiaSharp;

namespace eBooksGen.Libs
{
    /// <summary> Resizer record. </summary>
    /// <seealso cref="System.IEquatable&lt;eBooksGen.Libs.Resizer&gt;" />
    public sealed record Resizer(int Value, ResizeMode Mode = ResizeMode.LongSide, bool AllowEnlarge = true, bool AllowReduce = true)
    {
        /// <summary> Resizes the specified size. </summary>
        /// <param name="size"> The size. </param>
        /// <returns> </returns>
        public SKSize Resize(SKSize size)
        {
            int targetLength = GetLength(size, Mode);
            if (!AllowEnlarge && targetLength < Value)
            {
                return size;
            }

            if (!AllowReduce && targetLength > Value)
            {
                return size;
            }

            double ratio = 1.0 * Value / targetLength;
            int newWidth = (int)Math.Round(size.Width * ratio);
            int newHeight = (int)Math.Round(size.Height * ratio);
            return new SKSize(newWidth, newHeight);
        }

        /// <summary> Gets the length. </summary>
        /// <param name="size"> The size. </param>
        /// <param name="mode"> The mode. </param>
        /// <returns> </returns>
        private static int GetLength(SKSize size, ResizeMode mode)
        {
            return mode switch
            {
                ResizeMode.LongSide => (int)Math.Max(size.Width, size.Height),
                ResizeMode.ShortSide => (int)Math.Min(size.Width, size.Height),
                ResizeMode.Width => (int)size.Width,
                ResizeMode.Height => (int)size.Height,
                _ => throw new InvalidOperationException(nameof(GetLength)),
            };
        }
    }
}
