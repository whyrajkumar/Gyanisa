using Gyanisa.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Gyanisa.Sitemap
{
    public class SitemapMiddleware
    {
        private RequestDelegate _next;
        private string _rootUrl;
        public SitemapMiddleware(RequestDelegate next, string rootUrl)
        {
            _next = next;
            _rootUrl = rootUrl;
        }

        public async Task Invoke(HttpContext context)
        {
            //if (context.Request.Path.Value.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase))
            //{
                var stream = context.Response.Body;
                context.Response.StatusCode = 200;
                context.Response.ContentType = "application/xml";
                string sitemapContent = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
                var controllers = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type)
                    || type.Name.EndsWith("controller")).ToList();

                foreach (var controller in controllers)
                {
                    var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(method => typeof(IActionResult).IsAssignableFrom(method.ReturnType));
                    foreach (var method in methods)
                    {
                        sitemapContent += "<url>";
                        sitemapContent += string.Format("<loc>{0}/{1}/{2}</loc>", _rootUrl,
                        controller.Name.ToLower().Replace("controller", ""), method.Name.ToLower());
                        sitemapContent += string.Format("<lastmod>{0}</lastmod>", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                        sitemapContent += "</url>";
                    }
                }
                sitemapContent += "</urlset>";
                using (var memoryStream = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(sitemapContent);
                    memoryStream.Write(bytes, 0, bytes.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await memoryStream.CopyToAsync(stream, bytes.Length);
                }

                var xmlFile = "Sitemap5.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                FileStream fileStream = new FileStream(xmlPath, FileMode.Create);
                XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
                XmlWriter writer = XmlWriter.Create(fileStream, settings);
                XmlDocument document = new XmlDocument();

            
            document.Save(writer);
            document.Save(xmlFile);
            //document.LoadXml(xmlPath);

            //writer.Flush();
            //fileStream.Flush();
            // }
        }
    }

    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseSitemapMiddleware(this IApplicationBuilder app,string rootUrl = "http://www.tuitioner.in/")
        {   
            return app.UseMiddleware<SitemapMiddleware>(new[] { rootUrl });
        }
    }
}
