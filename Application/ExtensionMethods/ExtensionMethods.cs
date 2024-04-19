using Microsoft.AspNetCore.Http;

namespace Application.ExtensionMethods;

public static class ExtensionMethods
{
    public static byte[] GetBytes(this IFormFile formFile)
    {
        using(var memoryStream = new MemoryStream())
        {
            formFile.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }

    public static string SaveFile(this IFormFile formFile)
    {
        string rootPath = @"C:\Projekty\Blogger_Attachments";
        if(!Directory.Exists(rootPath))
        {
            Directory.CreateDirectory(rootPath);
        }

        string filePath = Path.Combine(rootPath, $"{Guid.NewGuid()}_{formFile.FileName}");

        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            formFile.CopyTo(fileStream);

        return filePath;
    }
}
