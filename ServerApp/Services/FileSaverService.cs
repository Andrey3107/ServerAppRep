using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class FileSaverService
{
    private readonly IWebHostEnvironment _environment;

    public FileSaverService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task Save(IFormFile file, string folderName)
    {
        try
        {
            string folderRoute = Path.Combine(_environment.WebRootPath, folderName);

            if (!Directory.Exists(folderRoute))
            {
                Directory.CreateDirectory(folderRoute);
            }

            string fileRoute = Path.Combine(folderRoute, file.FileName);

            using (FileStream fileStream = File.Create(fileRoute))
            {
                await file.OpenReadStream().CopyToAsync(fileStream);
            }
        }
        catch (Exception)
        {
            throw new HttpException("Faild to upload file!", 400);
        }
    }
}
