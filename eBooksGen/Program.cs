using eBooksGen.Libs;

string srcDirPath = args.FirstOrDefault() ?? string.Empty;
string dstDirPath = args.ElementAtOrDefault(1) ?? string.Empty;

if (!Directory.Exists(srcDirPath))
{
    Console.WriteLine($"not found. [{srcDirPath}]");
    Environment.Exit(1);
}

if (string.IsNullOrWhiteSpace(dstDirPath))
{
    Console.WriteLine($"invalid args. [{dstDirPath}]");
}

DirectoryInfo srcDir = new(srcDirPath);
DirectoryInfo dstDir = Directory.CreateDirectory(dstDirPath);

Resizer resizer = new(1920, ResizeMode.LongSide, false, true);
DirectoryInfo[] targetDirs = srcDir.GetDirectories("*", SearchOption.TopDirectoryOnly);
foreach (DirectoryInfo dir in targetDirs)
{
    string bookDirName = dir.Name;
    BookInfo bookInfo = BookTools.Parse(bookDirName);
    string jpgDirPath = Path.Combine(dstDir.FullName, bookDirName);
    if (Directory.Exists(jpgDirPath))
    {
        Directory.Delete(jpgDirPath, true);
    }

    DirectoryInfo jpgDir = Directory.CreateDirectory(jpgDirPath);
    FileInfo[] resized = ImageTools.Resize(dir, jpgDir, resizer.Resize).ToArray();
    Console.WriteLine($"{bookInfo.Name} resized {resized.Length} items.");

    string pdfName = $"{bookDirName}.pdf";
    string pdfPath = Path.Combine(dstDir.FullName, pdfName);
    FileInfo saved = PDFTools.Generate(bookInfo, resized.Select(e => e.FullName), pdfPath);
    Console.WriteLine($"saved. [{bookInfo.Name}]");
}

Environment.Exit(0);
