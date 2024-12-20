using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string image,string loctype);
        Task<string> SaveImage(IFormFile image, string loctype,string title);
        bool RemoveImage(string image, string loctype);
        bool RemoveImage(string image);

    }
}
