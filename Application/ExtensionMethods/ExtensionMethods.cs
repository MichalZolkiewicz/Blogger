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
}
