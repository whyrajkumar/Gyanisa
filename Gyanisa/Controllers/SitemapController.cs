using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Gyanisa.Data;
using Gyanisa.Data.Interface;
using Gyanisa.Data.Repository;
using Gyanisa.Extensions;
using Gyanisa.Helper;
using Gyanisa.Middleware;
using Gyanisa.Models;
using Gyanisa.Sitemap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Gyanisa.Controllers
{
    public class SitemapController : Controller
    {
        XmlDocument document = new XmlDocument();

        private readonly IHostingEnvironment _hostingEnvironment;
        protected readonly ApplicationDbContext _context;
        private readonly EmailConfig emailConfig;
        protected readonly IPostRepository _postRepository;
        protected readonly IConfiguration _configuration;
        public SitemapController(
            ApplicationDbContext context, 
            IHostingEnvironment hostingEnvironment, 
            IOptions<EmailConfig> options,
            IPostRepository postRepository,
            IConfiguration configuration)
        {
            this._context = context;
            _hostingEnvironment = hostingEnvironment;
            this.emailConfig = options.Value;
            this._postRepository = postRepository;
            _configuration = configuration;
        }


        [Route("sitemap")]
        public async Task<IActionResult> Sitemap()
        {   
             UserSitemap();
            await PostSitemap();
            return RedirectToAction("SitemapXml");
            //return  Content( xml, "text/xml");
        }
        public async Task PostSitemap()
        {   
            
            string baseUrl = MyHttpContext.AppBaseUrl;// $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
                                                
            var xmlPath = Path.Combine(AppContext.BaseDirectory) + "/sitemap_1.xml";

            var posts = await _postRepository.GetAllAsync();
            var siteMapBuilder = new SitemapBuilder();
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            // add the blog posts to the sitemap
            foreach (var post in posts)
            {
                //siteMapBuilder.AddUrl(baseUrl + post.Slug, modified: post.Published, changeFrequency: null, priority: 0.9);
               siteMapBuilder.AddUrl(baseUrl + "/posts/" + post.Id + "/" + post.Slug, modified: post.Created, changeFrequency: null, priority: 0.9);
            }
            //generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(xmlPath);

            //return RedirectToAction("Index", "Home");
        }

        public ContentResult UserSitemap()
        {
            

            string baseUrl = MyHttpContext.AppBaseUrl;// $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
            //var xmlPath = _configuration["Path:Sitemap"];
            var xmlPath = Path.Combine(AppContext.BaseDirectory) + "/sitemap_2.xml";

            var posts = _context.UserInformations.Where(p=>p.UserID!=1).ToList();
            // get last modified date of the home page
            var siteMapBuilder = new SitemapBuilder();
            // add the home page to the sitemap
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            // add the blog posts to the sitemap
            foreach (var post in posts)
            {
                siteMapBuilder.AddUrl(baseUrl + "/tutor/" + post.UserID + "/" + post.Slug, modified: post.ModifiedDate, changeFrequency: null, priority: 0.9);
            }
            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(xmlPath);
            return this.Content("");
        }


        public IActionResult Sitemapd1()
        {
            string baseUrl = "http://localhost:44315/";
           

            document.CreateAttribute("SITEMAP.XML");

            //get a list of publish Tutor of details
            var posts = _context.UserInformations.ToList();

            //    baseUrl = _hostingEnvironment.ContentRootPath;

            //get last modified date of the home page
            var sitemapbuilder = new SitemapBuilder();

            //add the home page to the sitemap
            sitemapbuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);

            //add the tutor post to the sitemap
            foreach (var post in posts)
            {
                sitemapbuilder.AddUrl(baseUrl + "/Details/" + post.UserID + "/" + post.Slug, modified: post.AddedDate, changeFrequency: null, priority: 0.9);
            }

            //generate the sitemap xml
            string xml = sitemapbuilder.ToString();

            //Locate the XML file being generated by ASP.NET...
            // var XxmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";

            var xmlFile = "Sitemap2.XML";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            //FileStream fileStream = new FileStream(xmlPath + xml, FileMode.Open);
            //document.copy(fileStream);

            FileStream fileStream = new FileStream(xmlPath, FileMode.Create);
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            XmlWriter writer = XmlWriter.Create(fileStream, settings);

            document.Save(fileStream);

            writer.Flush();
            fileStream.Flush();
            return RedirectToAction("Index", "Home");
            //return Content(xml, "text/xml");

        }


        [Route("robots.txt", Name = "GetRobotsText")]
        public ContentResult RobotsText()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /error/");
            stringBuilder.AppendLine("allow: /error/foo");
            stringBuilder.Append("sitemap: ");
            stringBuilder.AppendLine(this.Url.RouteUrl("GetSitemapXml", null, this.Request.Scheme).TrimEnd('/'));
            

            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }

       
        [Route("sitemap.xml", Name = "GetSitemapXml")]
        public ContentResult SitemapXml()
        {

            string baseUrl = MyHttpContext.AppBaseUrl;
            var siteMapBuilder = new SitemapBuilder();
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            // add the blog posts to the sitemap
            siteMapBuilder.AddUrl(baseUrl + "/home/about", modified: DateTime.UtcNow, changeFrequency: null, priority: 0.9);
            siteMapBuilder.AddUrl(baseUrl + "/home/contact", modified: DateTime.UtcNow, changeFrequency: null, priority: 0.9);
            siteMapBuilder.AddUrl(baseUrl + "/posts/blog", modified: DateTime.UtcNow, changeFrequency: null, priority: 0.9);
            siteMapBuilder.AddUrl(baseUrl +"/sitemap_1.xml", modified: DateTime.UtcNow, changeFrequency: null, priority: 0.9);
            siteMapBuilder.AddUrl(baseUrl + "/sitemap_2.xml", modified: DateTime.UtcNow, changeFrequency: null, priority: 0.9);

            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xml);
            //doc.Save(xmlPath);
            return Content(xml, "text/xml");

        }


        [Route("sitemap_1.xml")]
        public async Task<ContentResult> Sitemap1Xml()
        {
            string baseUrl = MyHttpContext.AppBaseUrl;// $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
            var xmlPath = Path.Combine(AppContext.BaseDirectory) + "/sitemap_1.xml";
            var posts = await _postRepository.GetAllAsync();
            var siteMapBuilder = new SitemapBuilder();
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            // add the blog posts to the sitemap
            foreach (var post in posts)
            {
                //siteMapBuilder.AddUrl(baseUrl + post.Slug, modified: post.Published, changeFrequency: null, priority: 0.9);
                siteMapBuilder.AddUrl(baseUrl + "/posts/" + post.Id + "/" + post.Slug+".htm", modified: post.Created, changeFrequency: null, priority: 0.9);
            }
            //generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            return Content(xml, "text/xml");
        }

        [Route("sitemap_2.xml")]
        public async Task<ContentResult> Sitemap2Xml()
        {

            string baseUrl = MyHttpContext.AppBaseUrl;// $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
            //var xmlPath = _configuration["Path:Sitemap"];
            var xmlPath = Path.Combine(AppContext.BaseDirectory) + "/sitemap_2.xml";

            var posts = await _context.UserInformations.Where(p => p.UserID != 1).ToListAsync();
            // get last modified date of the home page
            var siteMapBuilder = new SitemapBuilder();
            // add the home page to the sitemap
            siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            // add the blog posts to the sitemap
            foreach (var post in posts)
            {
                siteMapBuilder.AddUrl(baseUrl + "/tutor/" + post.UserID + "/" + post.Slug + ".htm", modified: post.ModifiedDate, changeFrequency: null, priority: 0.9);
            }
            // generate the sitemap xml
            string xml = siteMapBuilder.ToString();
            XmlDocument doc = new XmlDocument();
            return Content(xml, "text/xml");
        }
        public async Task<ContentResult> Sitemap3Xml()
        {
            string url = MyHttpContext.AppBaseUrl;
            var xmlpath = Path.Combine(AppContext.BaseDirectory) + "/sitemap_3.xml";
            var subjects = await _context.Subjects.ToListAsync();
            var siteMapBuilder = new SitemapBuilder();

            siteMapBuilder.AddUrl(url, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);
            foreach(var sub in subjects)
            {
              //  siteMapBuilder.AddUrl(url+"subject")
            }

            string xml = siteMapBuilder.ToString();
            return Content(xml,"text/xml");
        }


        
    }
}