using Gyanisa.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PhotoSauce.MagicScaler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.FileManager
{
    public class FileManager : IFileManager
    {
        private string _imagePath;
        private IConfiguration _config;
        public FileManager(IConfiguration config)
        {
            _config = config;
            //_imagePath = config["Path:Images"];
            //_userimagePath = config["Path:UserImages"];
        }

        //Using for Display Image
        public FileStream ImageStream(string image,string loctype)
        {
            try
            {
                if (loctype == "blog")
                {
                    _imagePath = _config["Path:Images"];
                }
                else if (loctype == "user")
                {
                    _imagePath = _config["Path:UserImages"];
                }
                var file = Path.Combine(_imagePath, image);
                if (!File.Exists(file))
                    if (loctype == "blog")
                    {
                        file = Path.Combine(_imagePath, "empty_blog.jpg");
                    }
                    else if (loctype == "user")
                    {
                        file = Path.Combine(_imagePath, "empty_user.png");
                    }

                return new FileStream(file, FileMode.Open, FileAccess.Read);

            }
            catch(FileNotFoundException ex)
            {  
                    return new FileStream(ex.Message, FileMode.Open, FileAccess.Read);

             


                //return new FileStream(Path.Combine(_imagePath, "empty_user.png"), FileMode.Open, FileAccess.Read);

            }
        }
        private static void LogException(Exception exception, bool expected = true)
        {
            Console.WriteLine($"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}");
        }

        //Using For Save Image
        public async Task<string> SaveImage(IFormFile image, string loctype,string title)
        {
            try
            {
                if (loctype == "blog")
                {
                    _imagePath = _config["Path:Images"];
                }
                else if (loctype == "user")
                {
                    _imagePath = _config["Path:UserImages"];
                }


                var save_path = Path.Combine(_imagePath);

                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }
                //Internet Explorer Error C:/User/Foo/image.jpg
                //var fileName=image.FileName;

                var fileName = "";
                var mine = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                if (loctype == "blog")
                {
                    fileName = title + $"_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mine}";
                }
                else if (loctype == "user")
                {
                   //.Replace("-", "") + Path.GetExtension(fileimg.FileName);
                    fileName = title + $"_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mine}";
                }

                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    //await image.CopyToAsync(fileStream);
                    if (loctype == "blog")
                    {
                        MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, BlogImageOptions());
                    }
                    else
                    {
                        MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageOptions());
                    }
                    
                }
                return  fileName;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
            
        }

        private ProcessImageSettings ImageOptions() => new ProcessImageSettings
        { 
            Width = 200,
            Height = 200,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 80,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420
        };

        private ProcessImageSettings BlogImageOptions() => new ProcessImageSettings
        {
            Width = 500,
            Height = 300,
            ResizeMode = CropScaleMode.Crop,
            SaveFormat = FileFormat.Jpeg,
            JpegQuality = 60,
            JpegSubsampleMode = ChromaSubsampleMode.Subsample420
        };


        public bool RemoveImage(string image, string loctype)
        {
            try
            {
                if (loctype == "blog")
                {
                    _imagePath = _config["Path:Images"];
                }
                else if (loctype == "user")
                {
                    _imagePath = _config["Path:UserImages"];
                }


                var file = Path.Combine(_imagePath, image);
                if (File.Exists(file))
                    File.Delete(file);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
       
        public void DeleteFile(string target_dir)
        {
            if (System.IO.File.Exists(target_dir) == true)
            {
                File.SetAttributes(target_dir, FileAttributes.Normal);
                File.Delete(target_dir);
            }

        }

        public bool  RemoveImage(string image)
        {
            try
            {
                

                var file = Path.Combine(_imagePath, image);
                if (File.Exists(file))
                    File.Delete(file);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }

}
