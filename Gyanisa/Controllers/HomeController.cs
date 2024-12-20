using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gyanisa.Models;
using Gyanisa.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Text;
using Gyanisa.Data.FileManager;
using Gyanisa.Models.ViewModel;
using System.Net.Mail;
using Gyanisa.Services;
using Gyanisa.Models.Blog;
using Gyanisa.Models.Admin;

namespace Gyanisa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _fileManager;
        private readonly IEmailSender _emailSender;
        
        public HomeController(ApplicationDbContext context,IFileManager fileManager, IEmailSender emailSender)
        {
            _context = context;
            _fileManager = fileManager;
            _emailSender = emailSender; 
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            IQueryable<Post> Result = _context.Posts.Select(aa => new Post
            {
                Id = aa.Id,
                Title = aa.Title,
                Body = aa.Body,
                Created = aa.Created,
                Image = aa.Image,
                Metaabstract = aa.Metaabstract,
                MetaDescription = aa.MetaDescription,
                MetaKeywords = aa.MetaKeywords,
                MetaTitle = aa.MetaTitle,
                Slug=aa.Slug,
                postGroup = new PostGroup
                {
                    PostGroupName = aa.postGroup.PostGroupName
                }
            }).AsQueryable().OrderBy(s=>s.Created);

            ViewBag.Newlist = Result;// await _context.Posts.ToListAsync();
            //var Result = await _context.Posts.ToListAsync();
           // List<Post> model = new List<Post>(result.AsQueryable().OrderByDescending(s => s.Created));
            return View(await Result.ToListAsync());
        }
        public IActionResult Indextutor()// this is for tutor Fature use
        {
            return View();
        }


        //[HttpGet]
        //public JsonResult SubjectResult(string Prefix)
        //{
        //    string term = HttpContext.Request.Query["term"].ToString();
        //    var Subjectlist = (from N in _context.Subjects
        //                       where N.SubjectName.StartsWith(term)
        //                       select new { N.SubjectName });
        //    return Json(Subjectlist.ToList());
        //}

        [HttpPost]
        public IActionResult SearchData(string Prefix)
        {
            var Subjectlist = (from N in _context.Subjects
                               where N.SubjectName.StartsWith(Prefix)
                               select new { N.SubjectName });
            return Json(Subjectlist.ToList());
        }

        public IActionResult AllTeacher()
        {
            var applicationDbContext = _context.UserInformations.Include(u => _context.Users.ToList());
            //var applicationDbContext =from c in  _context.userDetails
            var userdata = _context.Users.ToList();
            return View(applicationDbContext);
        }

        [Route("about.htm")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult TeacherDetails()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public ActionResult Teachers()
        {
            return View();
        }

        [Route("privacy-policy.htm")]
        public async Task<ActionResult> Privacy()
        {
            
            var termAndService = await _context.TermAndServices.OrderByDescending(u => u.Id).FirstOrDefaultAsync();

            if (termAndService == null)
            {
                throw new Exception("Details Not Found");
            }
             
            return View(termAndService);
        }
        [Route("disclaimer.htm")]
        public async Task<ActionResult> Disclaimer()
        {
            var termAndService = await _context.TermAndServices.OrderByDescending(u => u.Id).FirstOrDefaultAsync();

            if (termAndService == null)
            {
                throw new Exception("Details Not Found");
            }

            return View(termAndService);
        }

        [HttpGet] 
        public IActionResult Contact()
        {
           return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email,vm.Name);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("info@tuitioner.in");//Where mail will be sent 
                    msz.Subject = vm.Subject;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "mail.tuitioner.in";
                    smtp.Port = 25;
                    smtp.Credentials = new System.Net.NetworkCredential
                    ("info@tuitioner.in", "11naCg8*");
                    smtp.EnableSsl = false;
                    smtp.Send(msz);

                    // await _emailSender.SendEmailAsync(vm.Email, vm.Subject, vm.Message);


                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }
            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
 
       
     

    }
}
